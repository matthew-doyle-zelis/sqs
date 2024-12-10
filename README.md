using localstack, 

use awslocal

# Create queue
awslocal sqs create-queue --queue-name activity-queue

# List queues
awslocal sqs list-queues

# Get queue attributes
awslocal sqs get-queue-attributes --queue-url http://localhost:4566/000000000000/activity-queue --attribute-names All

# Send test message
awslocal sqs send-message --queue-url http://localhost:4566/000000000000/activity-queue --message-body "test"

# Receive messages
awslocal sqs receive-message --queue-url http://localhost:4566/000000000000/activity-queue