#! /bin/bash
# shellcheck disable=SC2125
PATH_FILTER=HaloBiz/*
CHANGED_FILES=$(git diff HEAD HEAD~ --name-only)
MATCH_COUNT=0

echo "Checking for file changes in Halobiz-backend..."
for FILE in $CHANGED_FILES
do

  if [[ $FILE ==  $PATH_FILTER ]];
  then
    # shellcheck disable=SC2016
    echo "MATCH:  ${FILE} changed"
    # shellcheck disable=SC2004
    MATCH_COUNT=$(($MATCH_COUNT+1))
  else
    echo "IGNORE: ${FILE}"
  fi
done

if [[ $MATCH_COUNT -gt 0 ]];
   then
    echo "$MATCH_COUNT match(es) for defined path '$PATH_FILTER' found in Halobiz-backend, build will proceed"
    echo "##vso[task.setvariable variable=SOURCE_CODE_CHANGED;isOutput=true]true"
  else
    echo "$MATCH_COUNT match(es) for defined path '$PATH_FILTER' not found in Halobiz-backend, build will not proceed"
    echo "##vso[task.setvariable variable=SOURCE_CODE_CHANGED;isOutput=true]false"
fi