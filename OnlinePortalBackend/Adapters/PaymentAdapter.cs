using Flurl.Http;
using Flutterwave.Net;
using HalobizMigrations.Data;
using HalobizMigrations.Models.OnlinePortal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlinePortalBackend.DTOs.AdapterDTOs;
using OnlinePortalBackend.DTOs.FlutterwaveDTOs;
using OnlinePortalBackend.DTOs.PaystackDTOs;
using OnlinePortalBackend.DTOs.ReceivingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlinePortalBackend.Adapters
{
    public interface IPaymentAdapter
    {
        public Task<VerifyPaymentResponse> VerifyPaymentAsync(PaymentGateway paymentType, string referenceCode);
    }

    public class PaymentAdapter : IPaymentAdapter
    {
        private readonly ILogger<PaymentAdapter> _logger;
        private readonly string _flutterWaveBaseUrl;
        private readonly string _flutterWaveSecretKey;
        private readonly string _paystackBaseUrl;
        private readonly string _paystackSecretKey;

        public PaymentAdapter(
            ILogger<PaymentAdapter> logger,
            IConfiguration config)
        {
            _logger = logger;

            _flutterWaveBaseUrl = config["FlutterwaveBaseUrl"] ?? config.GetSection("AppSettings:FlutterwaveBaseUrl").Value;
            _flutterWaveSecretKey = config["FlutterwaveSecretKey"] ?? config.GetSection("AppSettings:FlutterwaveSecretKey").Value;

            _paystackBaseUrl = config["PaystackBaseUrl"] ?? config.GetSection("AppSettings:PaystackBaseUrl").Value;
            _paystackSecretKey = config["PaystackSecretKey"] ?? config.GetSection("AppSettings:PaystackSecretKey").Value;
        }

        public async Task<VerifyPaymentResponse> VerifyPaymentAsync(PaymentGateway paymentType, string referenceCode)
        {
            var verifyPaymentResponse = new VerifyPaymentResponse();

            string responseString;


            switch (paymentType)
            {
                case PaymentGateway.Flutterwave:

                    try
                    {

                        string flutterwaveSecretKey = "FLWSECK_TEST-6a3f1783bea589e157df281ccc640782-X"; //ConfigurationManager.AppSettings["FlutterwaveSecretKey"];
                        var api = new FlutterwaveApi(flutterwaveSecretKey);
                        var trx = int.Parse(referenceCode);
                        var response = api.Transactions.VerifyTransaction(trx);

                        if (response.Status == "success")
                        {
                            verifyPaymentResponse.PaymentSuccessful = true;
                            return verifyPaymentResponse;

                        }
                        else
                        {
                            verifyPaymentResponse.PaymentSuccessful = false;
                            return verifyPaymentResponse;
                        }
                        return verifyPaymentResponse;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);

                        verifyPaymentResponse.PaymentSuccessful = false;
                        verifyPaymentResponse.Errors.Add(ex.Message);
                        return verifyPaymentResponse;
                    }

                case PaymentGateway.Paystack:

                    var payStackUrl = $"{_paystackBaseUrl}/verify/{referenceCode}";

                    responseString = await payStackUrl.AllowAnyHttpStatus()
                        .WithOAuthBearerToken(_paystackSecretKey)
                        .GetStringAsync();

                    try
                    {
                        var response = JsonConvert.DeserializeObject<dynamic>(responseString);

                        if (response.data.reference != referenceCode) verifyPaymentResponse.Errors.Add("Verification reference does not match.");
                        if (response.data.status != "success") verifyPaymentResponse.Errors.Add("Payment verification not successful.");
                        if (response.data.currency != "NGN") verifyPaymentResponse.Errors.Add("Payment currency not in Naira.");

                        if (verifyPaymentResponse.Errors.Any())
                        {
                            verifyPaymentResponse.PaymentSuccessful = false;
                            return verifyPaymentResponse;
                        }
                        else
                        {
                            verifyPaymentResponse.PaymentSuccessful = true;
                            verifyPaymentResponse.PaymentAmount = response.data.amount/100.0;
                            return verifyPaymentResponse;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);

                        verifyPaymentResponse.PaymentSuccessful = false;
                        verifyPaymentResponse.Errors.Add(ex.Message);
                        return verifyPaymentResponse;
                    }

                default:
                    verifyPaymentResponse.PaymentSuccessful = false;
                    verifyPaymentResponse.Errors.Add("Invalid Payment Type.");
                    return verifyPaymentResponse;
            }
        }
    }
}
