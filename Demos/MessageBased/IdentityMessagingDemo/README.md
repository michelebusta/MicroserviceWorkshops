# README - identity messaging demo

This demo illustrates how to send messages to a variety of targets including:

* RabbitMQ in a local Docker container
* Kafka in a local Docker container
* Azure Service Bus for a deployed scenario
* Azure Event Hubs for a deployed scenario

This demo also illustrates how to store information in a variety of document databases including:

* Postgres in a local Docker container
* Azure Cosmos DB for a deployed scenario

The goal is to illustrate how you can locally test end-to-end without Azure resources and offline, while flipping to Azure resources of equivalent capabilities. This sample does not provide a production ready provider pattern that would be desirable for real product code to completely remove any test / local equivalents while using Azure in the deployed environments. This simply illustrates the coding patterns to show how each technology works in its simplest form.

## Azure Setup

### Cosmos DB

Create a new Azure Cosmos DB Account with API set to **Core (SQL)**.

Once the resource is successfully created, access it's *Settings > Keys* information and gather the values for **URI** (named `{$COSMOS_URI}` onwards) and **PRIMARY KEY** (named `{$COSMOS_KEY}` onwards).

### Service Bus

Create a new Service Bus namespace with a pricing tier of, at least, **Standard**.

Once the resource is successfully created, access it's *Settings > Shared access policies* information, select the *RootManageSharedAccessKey* policy and gather the values for **Primary Key** (named `{SB_KEY}` onwards) and the endpoint address from **Primary Connection String** (named `{$SB_ENDPOINT}` onwards).

Access the resource *Entities > Topics* information and let's add a new topic:

* Click *+ Topic*
* Name the new topic **identitymessaging**
* Click **Create**

Once the topic is successfully created, select it and let's add two new subscriptions:

* Click *+ Subscription*
* Name the new subscription **identity-history**
* Click **Create**
* Click *+ Subscription*
* Name the new subscription **identity-messages**
* Click **Create**

### Event Hubs

Create a new Event Hubs namespace with a pricing tier of **Standard** (named `{$EH_NAMESPACE}` onwards).

Once the resource is successfully created, access it's *Settings > Shared access policies* information and let's add two new policies:

* Click *+ Add*
* Name the new policy **Producer**
* Select **Send**
* Click **Create**
* Click *+ Add*
* Name the new policy **Consumer**
* Select **Send** and **Listen**
* Click **Create**

Select the *Producer* policy and gather the value for **Primary Key** (named `{$EH_PRODUCER_KEY}` onwards)

Select the *Consumer* policy and gather the value for **Primary Key** (named `{$EH_CONSUMER_KEY}` onwards).

Access the resource *Entities > Event Hubs* information and let's add a new hub:

* Click *+ Event Hub*
* Name the new hub **eventdemos**
* Click **Create**

Once the hub is successfully created, select it, access it's *Entities > Consumer groups* information and let's add two new groups:

* Click *+ Consumer group*
* Name the new group **identity.history**
* Click **Create**
* Click *+ Consumer group*
* Name the new group **identity.runtime**
* Click **Create**

### Storage Account

Create a new Storage account.

Once the resource is successfully created, access it's *Settings > Access keys* information and gather the values **Storage account name** (named `{$SA_NAME}` onwards) and **key1 > Key** (named `{$SA_KEY}` onwards).

## Configuration Settings

In the projects **IdentityHistoryConsumer**, **IdentityRuntimeConsumer** and **IdentityManagementWeb** there is a configuration entry named **EventsSystem** that allows to specify which events system should be used by the system. The following table shows the valid values and the underlying events system it relates to:

| Value | Underlying system |
| - | - |
| rabbitmq | RabbitMQ |
| kafka | Kafka |
| servicebus | Azure Service Bus |
| eventhub | Azure Event Hubs |

### IdentityManagementApi

To configure the access to the document database, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| DocumentDbConfig:DbBackend | Marten | There are two valid values: `Marten` and `CosmosDb` |
| DocumentDbConfig:CosmosConfig:ServiceEndpoint | https://example.documents.azure.com:443/ | The Cosmos DB endpoint hold by `{$COSMOS_URI}` |
| DocumentDbConfig:CosmosConfig:AuthKey | NiHYb4iTWD(...)DbXWCxaFnTKQ== | The Cosmos DB access key hold by `{$COSMOS_KEY}` |
| DocumentDbConfig:CosmosConfig:Database | IdentityMessagingDemo | The name of the Cosmos DB database |
| DocumentDbConfig:CosmosConfig:Collection | Management | The name of the collection to use in the Cosmos DB database |
| DocumentDbConfig:MartenConfig:UserId | postgres | The Postgres user id |
| DocumentDbConfig:MartenConfig:Password | Password1 | The Postgres user password |
| DocumentDbConfig:MartenConfig:Host | localhost | The hostname of the Postgres instance |
| DocumentDbConfig:MartenConfig:Port | 5432 | The port number of the Postgres instance |
| DocumentDbConfig:MartenConfig:Database | Management | The name of the Postgres database |

### IdentityManagementWeb

To configure the access to the **RabbitMQ** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| RabbitMQProducerConfig:HostName | localhost | The hostname of the RabbitMQ instance |

To configure the access to the **Kafka** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| kafka:bootstrap.servers | localhost:9092 | The endpoint of the Kafka instance |

To configure the access to the **Azure Service Bus** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| ServiceBusProducerConfig:AuthKeyName | RootManageSharedAccessKey | The access policy name |
| ServiceBusProducerConfig:AuthKey | j7CCUzK3L8Oq2QavR54lIxya8ZVTqEM8OrdEU1eDm2Y= | The access key hold by `{SB_KEY}` |
| ServiceBusProducerConfig:Topic | identitymessaging | The topic name |
| ServiceBusProducerConfig:EndpointAddress | sb://example.servicebus.windows.net/ | The endpoint hold by `{$SB_ENDPOINT}` |

To configure the access to the **Azure Event Hubs** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| EventHubProducerConfig:Namespace | identityeventsexample | The name hold by `{$EH_NAMESPACE}` |
| EventHubProducerConfig:AuthKeyName | Producer | The access policy name |
| EventHubProducerConfig:AuthKey | YsapOYdVnZv54BBXy45NCnrFXMt2uAzVyHfxsXg0xEY= | The access key hold by `{$EH_PRODUCER_KEY}` |

### IdentityHistoryConsumer

To configure the access to the document database, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| DocumentDbConfig:DbBackend | Marten | There are two valid values: `Marten` and `CosmosDb` |
| DocumentDbConfig:CosmosConfig:ServiceEndpoint | https://example.documents.azure.com:443/ | The Cosmos DB endpoint hold by `{$COSMOS_URI}` |
| DocumentDbConfig:CosmosConfig:AuthKey | NiHYb4iTWD(...)DbXWCxaFnTKQ== | The Cosmos DB access key hold by `{$COSMOS_KEY}` |
| DocumentDbConfig:CosmosConfig:Database | IdentityMessagingDemo | The name of the Cosmos DB database |
| DocumentDbConfig:CosmosConfig:Collection | History | The name of the collection to use in the Cosmos DB database |
| DocumentDbConfig:MartenConfig:UserId | postgres | The Postgres user id |
| DocumentDbConfig:MartenConfig:Password | Password1 | The Postgres user password |
| DocumentDbConfig:MartenConfig:Host | localhost | The hostname of the Postgres instance |
| DocumentDbConfig:MartenConfig:Port | 5432 | The port number of the Postgres instance |
| DocumentDbConfig:MartenConfig:Database | History | The name of the Postgres database |

To configure the access to the **RabbitMQ** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| RabbitMQConsumerConfig:HostName | localhost | The hostname of the RabbitMQ instance |
| RabbitMQConsumerConfig:Group | identity.history | The group name |

To configure the access to the **Kafka** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| kafka:bootstrap.servers | localhost:9092 | The endpoint of the Kafka instance |
| kafka:group.id | identity.history | The group name |
| kafka:enable.auto.commit | false | If auto commit is enable or disable |
| kafka:default.topic.config:auto.offset.reset | earliest | The offset reset policy |

To configure the access to the **Azure Service Bus** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| ServiceBusConsumerConfig:AuthKeyName | RootManageSharedAccessKey | The access policy name |
| ServiceBusConsumerConfig:AuthKey | j7CCUzK3(...)OrdEU1eDm2Y= | The access key hold by `{SB_KEY}` |
| ServiceBusConsumerConfig:Topic | identitymessaging | The topic name |
| ServiceBusConsumerConfig:Subscription | identity-history | The topic subscription name |
| ServiceBusConsumerConfig:EndpointAddress | sb://example.servicebus.windows.net/ | The endpoint hold by `{$SB_ENDPOINT}` |

To configure the access to the **Azure Event Hubs** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| EventHubConsumerConfig:Namespace | identityeventsexample | The name hold by `{$EH_NAMESPACE}` |
| EventHubConsumerConfig:AuthKeyName | Consumer | The access policy name |
| EventHubConsumerConfig:AuthKey | /Pm9cunIEl(...)OJuW19Vc1MDA= | The access key hold by `{$EH_CONSUMER_KEY}` |
| EventHubConsumerConfig:Group | identity.history | The group name |
| EventHubConsumerConfig:StorageAccountName | identityeventsexamplestorage | The name of the storage account to be used by the client library hold by `{$SA_NAME}` |
| EventHubConsumerConfig:StorageAccountKey | iCGMKKUOa6(...)y5oxoFMZdGXwJA== | The storage account access key to be used by the client library hold by `{$SA_KEY}` |
| EventHubConsumerConfig:StorageContainerName | identitymessagingdemohistory | The name of the storage account container to be used by the client library |

### IdentityRuntimeConsumer

To configure the access to the **RabbitMQ** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| RabbitMQConsumerConfig:HostName | localhost | The hostname of the RabbitMQ instance |
| RabbitMQConsumerConfig:Group | identity.runtime | The group name |

To configure the access to the **Kafka** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| kafka:bootstrap.servers | localhost:9092 | The endpoint of the Kafka instance |
| kafka:group.id | identity.runtime | The group name |
| kafka:enable.auto.commit | false | If auto commit is enable or disable |
| kafka:default.topic.config:auto.offset.reset | earliest | The offset reset policy |

To configure the access to the **Azure Service Bus** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| ServiceBusConsumerConfig:AuthKeyName | RootManageSharedAccessKey | The access policy name |
| ServiceBusConsumerConfig:AuthKey | j7CCUzK3(...)OrdEU1eDm2Y= | The access key hold by `{SB_KEY}` |
| ServiceBusConsumerConfig:Topic | identitymessaging | The topic name |
| ServiceBusConsumerConfig:Subscription | identity-messages | he topic subscription name |
| ServiceBusConsumerConfig:EndpointAddress | sb://example.servicebus.windows.net/ | The endpoint hold by `{$SB_ENDPOINT}` |

To configure the access to the **Azure Event Hubs** instance, make use of the following entries:

| Name | Example | Description |
| - | - | - |
| EventHubConsumerConfig:Namespace | identityeventsexample | The name hold by `{$EH_NAMESPACE}` |
| EventHubConsumerConfig:AuthKeyName | Consumer | The access policy name |
| EventHubConsumerConfig:AuthKey | /Pm9cunIEl(...)OJuW19Vc1MDA= | The access key hold by `{$EH_CONSUMER_KEY}` |
| EventHubConsumerConfig:Group | identity.runtime | The group name |
| EventHubConsumerConfig:StorageAccountName | identityeventsexamplestorage | The name of the storage account to be used by the client library hold by `{$SA_NAME}` |
| EventHubConsumerConfig:StorageAccountKey | iCGMKKUOa6(...)y5oxoFMZdGXwJA== | The storage account access key to be used by the client library hold by `{$SA_KEY}` |
| EventHubConsumerConfig:StorageContainerName | identitymessagingdemoruntime | The name of the storage account container to be used by the client library |