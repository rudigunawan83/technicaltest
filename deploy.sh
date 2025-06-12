#!/usr/bin/env bash

set -e
set -o pipefail

if [ $# -eq 0 ]
  then
    echo "Usage: deploy.sh [version] [namespace] [service]"
    exit 1
fi

cat service.yaml | sed 's/\$BUILD_NUMBER'"/$1/g" | sed 's/\$NAMESPACE'"/$2/g" | kubectl apply -n $3 -f - --kubeconfig=kubeconfig.conf