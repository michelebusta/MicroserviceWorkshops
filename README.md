# Workshop Lab README

This folder contains setup instructions to set up ahead of my microservices workshop labs.
If you are just looking to learn how to set up an environment on any of the below platforms, you can also feel free to follow these setup steps but the labs themselves are only shared with attendees with the exception of the public labs produced for Microsoft by our team, which are now public domain as noted here.

> UPCOMING WORKSHOPS:

```text
February 26 and 27, 2019
2-day workshop
Surviving development, DevOps and production with Docker and Kubernetes
NOTE: there is limited time for hands on in the workshop but the labs are there to get started with.
Relevant labs: Azure AKS, AWS AKS
```

https://ndcporto.com/workshop/surviving-development-devops-and-production-with-docker-and-kubernetes

-----
ISSUES:

```text
#1 issue running content-web
Change the Dockerfile base image to use node:8 instead of node:argon

```
-----

In ALL, there are 5 different versions of the same lab. You have to choose a platform to focus on for the workshop since you clearly can't do all of them :)
The idea is to give you a choice of platform you are most interested in during the class period but you may optionally do the rest later.

> NOTE: these labs are maintained based on demand for workshops so some may be more current than others, be aware that the steps do change and screenshots become out of date. Any lab actively delivered in a workshop, is up to date before that workshop :)

## Choose your platform for the workshop lab

You should follow the setup steps in this folder to set up your environment. When the workshop is fully hands-on you want to do this BEFORE the workshop. Sometimes the labs are used for your own time given the content that needs to be covered.

Each workshop focuses on different platforms, and this will be indicated in the workshop link so that you know which labs to choose from. I check the relevant labs before a hands on workshop so the pace of "update" depends on the workshop activity. I keep each workshop indicating it's last "review" date.

You'll choose from the following environments:

* Azure Container Service (AKS) with Kubernetes
* AWS with EKS
* AWS with EC2 Container Services
* Google Cloud with Google Container Engine
* Azure Container Service with DC / OS
* Azure Service Fabric
* Azure with Docker Enterprise Edition (COMING SOON)

> NOTE: for the AKS lab, just follow the one folder, now that it is public domain the structure has changed.

## Prerequisite setup steps

You must have the setup steps in this folder completed before the lab period if you want to be productive during lab time.

Also, during the lab period, you will be given access to the actual lab instructions to follow.
You must follow the steps EXACTLY as written to avoid running into issues during lab time.

## Lab goals

When you get to the workshop.

A document describing the steps for each lab will be provided. You will be given some time to do work in the workshop but also to continue offline at your own pace.

The goal is to show similar deployments of a simple web app / api to a container platform whereby we go through the following:

1. Create and run the app
2. Dockerize the app and test locally
3. Deploy the application to a container platform
4. Work with load balancing, service discovery
5. Work with load testing, monitoring, scale out

## Bit.ly links to lab starter files

See lab setup steps for the correct starter files.
