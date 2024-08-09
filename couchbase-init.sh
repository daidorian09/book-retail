#!/bin/bash

# Wait for Couchbase to start
sleep 30

# Define bucket creation commands
create_bucket() {
  local bucket_name=$1
  local bucket_password=$2
  local bucket_memory=$3

  curl -X POST \
    -u Administrator:Administrator \
    -H "Content-Type: application/json" \
    -d '{
      "name": "'"$bucket_name"'",
      "ramQuotaMB": '"$bucket_memory"',
      "authType": "sasl",
      "saslPassword": "'"$bucket_password"'"
    }' \
    http://localhost:8091/pools/default/buckets
}

# Create buckets
create_bucket "order" "Administrator" 100
create_bucket "book" "Administrator" 100
create_bucket "customer" "Administrator" 100

# Wait for buckets to be created
sleep 30

# Define index creation function
create_index() {
  local bucket_name=$1
  local index_name=$2
  local fields=$3

  curl -X POST \
    -u Administrator:Administrator \
    -H "Content-Type: application/json" \
    -d '{
      "type": "GSI",
      "name": "'"$index_name"'",
      "bucket": "'"$bucket_name"'",
      "indexKey": ["'"$fields"'"]
    }' \
    http://localhost:8091/analytics/indexes
}

# Create indexes
create_index "customer" "customerEmail" "email"
create_index "order" "customerOrderById" "customer.id"
create_index "order" "orderDate" "orderDate"
