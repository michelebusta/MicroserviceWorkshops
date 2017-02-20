# AWS Lab Setup
This document describes all of the preparation you should take care of prior to the workshop.
If you do not have time, you may work on this during the workshop but may not complete as many lab steps
during lab times and can work on your own time to complete the lab at your leisure.

# Prerequisites
The lab will make use of several tools and services.  Here is a list of requirements that you will need to successfully complete the lab.  If you are missing any of these requirements, we will cover the steps to satisfy them in this document.

* An internet connection.
* Amazon Web Services Account.
	* You will need an email or mobile number
	* You will need to enter credit card information
* A DockerHub account
	* You will need an email address
* A browser, preferably Chrome for consistency with this guide.
* A command prompt (some commands will work in windows CMD but for consistency we will use bash by default).
	* You will need openssh at a minimum
	* You will need the aws cli 

# Preparation Steps in this Document

**Create your AWS Account**
* Task 1: Create an Amazon Web Services account 
* Task 2: Install AWS CLI (Linux/Mac) 
* Task 3: Install AWS CLI (Windows)
* Task 4: Setup IAM Access 

**Create a cluster and supporting tooling / setup (30 minutes)**
* Task 1: Install Git tools
* Task 2: Create an SSH key
* Task 3: Upload public key to EC2
* Task 4: Create a build agent VM
* Task 5: Create an AWS Resource Group
* Task 6: Add Resource tag to the Security Group
* Task 7: Connect securely to the build agent
* Task 8: Complete the build agent setup
* Task 9: Create a Docker Hub account 
* Task 10: Create an EC2 Container Service cluster
* Task 11: Cleanup sample app

#Create an AWS Account

##Task 1: Create an Amazon Web Services account

**Duration:** 20-30 minutes

1. Go to the account signup page: <https://aws.amazon.com>

 * If you are new to this site it will show a button in the header "Create AWS Account", click this.
 * If you have been here before it will how a button in the header "Sign In to the Console", click this.

To create your account (assuming you don't have one):

* Enter email or mobile number.
* Choose “I am a new user.”
* Click “Sign in using our secure server”

![AWS Sign In](images/image1.jpg)

2. Enter your name, and retype your email address. Choose a password.

    ![AWS Login Credentials](images/image2.jpg)

2. Choose personal or company account. 
    Then Fill out the required information. Click “Create Account and Continue”. **Tip: Do not include any whitespace when entering the CAPTCHA characters.**

    ![AWS Contact Information](images/image3.jpg)

3. Next enter payment information.

    ![AWS Payment Information](images/image4.jpg)
    
4. Next provide a telephone number so that Amazon can verify your identity.

    ![AWS Identity Verification](images/image5.jpg)

    * When the call arrives, enter the pin as displayed

       ![AWS Enter Pin](images/image6.jpg)

    * Now click “Continue”

        ![AWS Verification Complete](images/image7.jpg)

5. Choose the basic support plan.

    ![AWS Support Plan](images/image8.jpg)

6. You should be returned to the AWS home page. Click “Sign In to the
Console”

    ![AWS Home](images/image9.jpg)

7. Now choose that you are a returning user and enter your password. Click the sign-in button.

    ![AWS Sign In](images/image10.jpg)

8. You should be logged in to the console.

    ![AWS Console Home](images/image11.jpg)

##Task 2: Install AWS CLI (Linux/Mac)
**Duration:** 5-10 minutes

**Instructions**
<http://docs.aws.amazon.com/cli/latest/userguide/installing.html#install-bundle-other-os>

1. Check python install

    `$ python --version`

    This should print out version info, otherwise you must install python.

2. Download the installer:

    `$ curl "https://s3.amazonaws.com/aws-cli/awscli-bundle.zip" -o "awscli-bundle.zip"`

3. Unpack the bundle:

    `$ unzip awscli-bundle.zip`

4. Run the installer:

    `$ sudo ./awscli-bundle/install -i /usr/local/aws -b /usr/local/bin/aws`

5. Test your installation:

    `$ aws -version`

    This should print version information.

## Task 3: Install AWS CLI (Windows)
**Duration:** 5-10 minutes

1. Go to:
<http://docs.aws.amazon.com/cli/latest/userguide/installing.html#install-msi-on-windows>

1. Download the appropriate installer (32 vs 64-bit)

2. Start the installation wizard click through the next few screens, accept default options if any.

    ![AWS Wizard](images/image12.jpg)

    ![AWS EULA](images/image13.jpg)

    ![AWS Features](images/image14.jpg)

    ![AWS Install](images/image15.jpg)

    ![AWS Wait](images/image16.jpg)

    ![AWS Finish](images/image17.jpg)

3. Verify installation.

    * Start a terminal by clicking the windows menu and typing “cmd” then pressing enter.
    * Test installation by printing version information

        ![AWS Version Info](images/image18.jpg)
        
##Task 4: Setup IAM Access
**Duration:** 20-25 minutes

We will create an account admin for the purposes of running the examples in this workshop. 
The account admin can be deleted after the workshop if you choose.

1. Login to the AWS console: <https://console.aws.amazon.com>

2. Type IAM in the search box:

    ![AWS IAM Search](images/image19.jpg)

3. Click the search result to go to Identity and Access Management. Then
click on “Users”

    ![Click Users](images/image20.jpg)

4. Click “Add user”

    ![Click Add User](images/image21.jpg)

5. Enter “workshop-admin” as the user name. Choose “Programmatic access” as the access type. Click next.

    ![User Details](images/image22.jpg)
    
6. Choose “Attach existing policies directly”, then search for the policy named “AdministratorAccess” check the checkbox next to the policy. Click
next.

    ![User Permissions](images/image23.jpg)
    
7. Review the configuration, then click “Create User”

    ![User Review](images/image24.jpg)
    
    **Important:** Amazon will only display the secret access key once--on this screen. Do not click close until you have captured the key.

    Click “Download .csv” and a file will download which contains the key.

    ![Download CSV](images/image25.jpg)
    
    Alternatively, click “Show” and the key will be shown inline.

    ![Show Key](images/image26.jpg)
    
8. Now use the keys to configure the AWS cli. Open your terminal/command
prompt and run “aws configure”

    `$ aws configure`

    * Enter the Access Key ID when prompted.
    * Enter the Secret Access Key when prompted.
    * Use enter to accept the remaining defaults.

9. Test the configuration by describing ec2 instances.

    `$ aws ec2 describe-instances --region us-west-2`

10. There shouldn’t be any yet, but the command should succeed

    ![Describe instances](images/image27.jpg)
    
#Before the Workshop
**Duration**: 30 minutes (possibly additional time if AWS provisioning is slower)

##Task 1: Install Git tools

In this section, you will install Git tools. 
This documentation assumes you are installing Git tools for Windows but you may use another platform to achieve the same results at your own discretion.

1.  Navigate to <https://git-scm.com/download> and select Windows. This will download the latest Git tools executable.

    ![Git Download](images/ex0-image5.jpg)
    
2. Run the executable, click Next to pass the license dialog, and accept the default Git folder path.

3. On each dialog, choose the settings as shown in the following screen shots, and click Next until reaching the final screen where you will click Install.

    ![Git Components](images/ex0-image6.jpg)

    ![Git Start Menu](images/ex0-image7.jpg)

    ![Git PATH configuration](images/ex0-image8.jpg)

    ![Git line endings](images/ex0-image9.jpg)

    ![Git terminal emulator](images/ex0-image10.jpg)

    ![Git extra options](images/ex0-image11.jpg)

    ![Git extra options](images/ex0-image12.jpg)

    ![Git experimental options](images/ex0-image13.jpg)

4.  When the installation is completed, check Launch Git Bash and click Finish to show the command line window.

    ![Git finish setup](images/ex0-image14.jpg)

5. At the end of these steps you should see the Git Bash command line window.

    ![Git Bash window](images/ex0-image15.jpg)
    
##Task 2: Create an SSH key

In this section, you will create an SSH key to securely access the VMs you create during the upcoming exercises.

1.  Open Git Bash to access the command line tool.

    ![Git Bash Shortcut](images/ex0-image16.jpg)
    
2.  From the command line enter the following command to generate an SSH key pair. You must use “ubuntu” as the comment data.

    `$ ssh-keygen -t RSA -b 2048 -C ubuntu`

    * When prompted enter “fabmedical\_rsa” as the key file name.
    *  Enter a passphrase when prompted, and don’t forget it!

3.  The file will be generated in your user folder where Git Bash opens by default, so be sure to take note of the path.

    ![SSH Key Gen](images/ex0-image17.jpg)
    
4. Navigate to your user folder at c:\\Users\\\[your username\]. You will see the file matching the name you provided.

    ![SSH Keys](images/ex0-image19.jpg)

    
## Task 3: Upload public key to EC2

In this section, you will upload the public portion of the key pair you just created to EC2. This allows AWS to assign the key to resources as it creates them, which in turn allows you to authenticate with those resources using the private key.

1.  From the AWS Console type “EC2” in the services search box. Select the “EC2” search result.

    ![EC2 Search](images/ex0-image21.jpg)

2.  On the EC2 Dashboard, click the “Key Pairs” item on the left menu.

    ![EC2 Key Pairs](images/ex0-image22.jpg)
    
3.  Click on “Import Key Pair”.

    ![Import Key Pair](images/ex0-image23.jpg)
    
4.  Choose “fabmedical\_rsa.pub” and the name and contents will be automatically populated. You should see “ssh-rsa” at the beginning of the content block. If you do not, you may have picked the incorrect file. Click Import.

    ![Import Key Pair Dialog](images/ex0-image24.jpg)
    
5. Your key should now be available.

    ![Key Pairs](images/ex0-image25.jpg)
    
## Task 4: Create a build agent VM

In this section, you will create a Linux VM to act as your build agent. You will be installing Docker to this VM once it is set up and you will use this VM during the lab to develop and deploy.

**NOTE: You can set up your local machine with Docker however the setup varies for different versions of Windows.** **For this** **lab, the build agent approach simply allows for predictable setup.**

1.  Navigate to the EC2 Dashboard. On the EC2 Dashboard, click “Launch Instance”

    ![Launch Instance](images/ex0-image26.jpg)
    
2.  Check the checkbox for “Free tier only” then choose “Ubuntu Server 16.04 LTS”.

    ![Choose AMI](images/ex0-image27.jpg)
    
3.  Accept the default choice for instance type: t2.micro. Click Next.

    ![Choose Instance Type](images/ex0-image28.jpg)
    
4.  Accept all the default parameters for “Step 3” and click Next.

5.  Accept all the default parameters for “Step 4” and click Next.

6.  Add a name tag with the value “build-agent”. Add another tag. Use “Resource” as the key and “fabmedical” as the value. Click Next.

    ![Add Tags](images/ex0-image29.jpg)
    
7.  In Step 6 we will create a security group, but we need to give it a proper name. Enter “fabmedical-build-agent” as the security group name and enter “Allow SSH access” as the description. Click “Review and Launch”.

    ![Configure Security Groups](images/ex0-image30.jpg)
    
8.  Click launch to launch the VM. The final piece of configuration will be choosing the key pair you would like to use to access the VM. Choose “fabmedical\_rsa” the key you imported previously. You will not be able to click “Launch Instances” until you acknowledge that you have access to the private key.

    ![Select Key Pair](images/ex0-image31.jpg)
    
9.  The VM will begin deployment into your default VPC.

    ![Launch Status](images/ex0-image38.jpg)
    
10. When the VM is provisioned you will see it in your list of instances.

    ![EC2 Instance List](images/ex0-image41.jpg)
    
## Task 5: Create an AWS Resource Group
	
1. Login to console.
2. Choose "Resource Groups" from the top menu bar.

   ![Resource Group](images/RG1.jpg)
	
3. Choose "Create a Resource Group"
 
   ![Create Resource Group](images/RG2.jpg)
	
4. Configure as described below, then click "Save".
	1. Use "fabmedical Resources" as the group name.
	2. For Tags, type "Resource" as the key and "fabmedical" as the value.

     ![Resource Group Configuration](images/RG3.jpg)

5. The Resource Group is created

   ![Resource Group Created](images/RG4.png)

## Task 6: Add Resource tag to the Security Group

In this section, you will add a Resource tag to the fabmedical-build-agent security group. Because we created the security group using the launch wizard, we were not able to set tags. This can make it easy to forget to clean up the security group, because it will not show up in a resource group search.

1.  Navigate to EC2 and click on “Security Groups”

    ![Security Groups](images/ex0-image43.jpg)
    
2.  Select the security group with the group name “fabmedical-build-agent”

    ![Select Security Groups](images/ex0-image44.jpg)
    
3.  Click “Tags” in the bottom half of the screen. Then click “Add/Edit
    Tags”

    ![Edit Tags](images/ex0-image45.jpg)
    
4.  Click “Create Tag”, then enter “Resource” as the key, and “fabmedical” as the value. Then click “Save”.

    ![Create Tag](images/ex0-image46.jpg)
    
5. The resource tag appears in the list of the security group’s tag.

    ![Security Group Tags](images/ex0-image47.jpg)
    
## Task 7: Connect securely to the build agent

In this section, you will validate that you can connect to the new build agent VM.

1.  From the AWS Console, navigate to the Resource Group you created previously and select the new VM, build-agent. Click the “Go” link.

    ![Resource Group](images/ex0-image48.jpg)
    
2.  In the bottom portion of the EC2 instance list, take note of the public IP address for the VM.

    ![VM IP Address](images/ex0-image49.jpg)
    
3.  From your local machine, launch Git Bash and navigate to your user directory c:\\Users\\\[your username\] where the key pair was previously created.

4.  Connect to the new VM you created by typing the following command.

    `ssh -i \[PRIVATEKEYNAME\] \[BUILDAGENTUSERNAME\]@\[BUILDAGENTIP\]`

    Use the private key name such as “fabmedical\_rsa”, the username for the VM such as “ubuntu”, and the IP address for the build agent VM.

    `$ ssh -i fabmedical\_rsa ubuntu@54.202.82.171`

5.  You will be asked to confirm if you want to connect, as the authenticity of the connection cannot be validated. Type “yes”.

6.  You will be asked for the pass phrase for the private key you created previously. Enter this value.

7. You will now be connected to the VM with a command prompt such as the following. Keep this command prompt open for the next step.

    `ubuntu@ip-172-31-16-103:~\$`

    ![SSH Remote Session](images/ex0-image51.jpg)
    
    **NOTE: If you have issues connecting, you may have pasted the imported the SSH public key** **into EC2** **incorrectly.** **Unfortunately, if this is the case, you** **must** **retry the import, then try to create the VM again.**

## Task 8: Complete the build agent setup

In this task, you will update the packages and install Docker engine.

1.  Go to Git Bash, with the connection open to the build agent VM.

2.  Update the Ubuntu packages and install Docker engine, curl, node.js and the node package manager in a single step by typing the following in a single line command.

    `$ sudo apt-get update && sudo apt-get install -y docker.io curl nodejs npm`

3.  Next install bower.

    `$ sudo apt install -y nodejs-legacy && sudo npm install -g bower`

4.  When the command has completed, check the Docker version installed by executing this command. The output may look something like that shown in the following screen shot. Note that the server version is not shown yet, because you didn’t run the command with elevated privileges (to be addressed shortly).

    `docker version`

    ![Docker Version Output](images/ex0-image53.jpg)
    
5.  You may check the versions of node.js and npm as well, just for information purposes, using these commands.

    `nodejs --version`

    `npm -version`

6.  Add your user to the Docker group so that you do not have to elevate privileges with sudo for every command. You can ignore any errors you see in the output.

    `sudo usermod -aG docker $USER`

    NOTE: You may see the following error messages when running the sudo command. They can be ignored.

    ![Usermod Errors](images/ex0-image54.jpg)

7.  For the user permission changes to take effect, exit the SSH session by typing ‘exit’, then press &lt;Enter&gt;. Repeat the commands in Task 5 from step 4 to establish the SSH session again.

8.  Now, run the Docker version command again, and note the output now shows the server version as well.

    ![Second Docker Version Output](images/ex0-image55.jpg)

9.  Run a few Docker commands.

    * One to see if there are any containers presently running

    -   One to see if any containers exist whether running or not

-   In both cases, you will have an empty list but no errors running the command. Your build agent is ready with Docker engine running properly.

    `docker ps`

    `docker ps -a`

    ![Docker ps output](images/ex0-image56.jpg)
    
## Task 9: Create a Docker Hub account
Docker images are deployed from a Docker Registry. 
To complete the lab, you will need access to a registry that is publicly accessible to the Google Cloud cluster you are creating. 
In this task, you will create a free Docker Hub account for this purpose, where you push images for deployment.

**NOTE: If you already have a public Docker Hub account you do not need to complete this task – you will need the name of the account for the exercises that rely on it.**

1.  Navigate to Docker Hub at <https://hub.docker.com>.

2.  Create a new Docker Hub account by providing a unique name for your Docker Hub ID, your email address for confirmation of the account, and a password.

    ![New to Docker](images/ex0-image57.jpg)
    
3.  Once you confirm your email address, you can sign in and confirm your Docker Hub account is ready.

    ![Confirm email address](images/ex0-image58.jpg)

4. Login to your Docker Hub account. If it is a new account, you will not see any repositories yet. You will create these during the lab.

    ![Welcome to DockerHub](images/ex0-image59.jpg)
    
## Task 10: Create an EC2 Container Service cluster

In this task, you will create your EC2 Container Service cluster. You will use the same SSH key you created previously to connect to this cluster in the next task.

1.  From the AWS Console home page search for EC2. Choose “EC2 Container Service”.

    ![ECS Search](images/ex0-image60.jpg)
    
2.  On the EC2 Container Service getting started page, click “Get started”.

    ![ECS getting started](images/ex0-image61.jpg)
    
3.  Choose to deploy the sample application to a new cluster. Uncheck the box for Amazon ECR.

    ![ECS options](images/ex0-image62.jpg)
    
4.  Accept the default options for the task definition and click next.

5.  Accept the default options for the service configuration and click next.

6.  Configure the cluster. Then click “Review & launch”.

    a.  Use “fabmedical” as the cluster name.

    b.  Set number of instances to “2”.

    c.  Choose “fabmedical\_rsa” as the key pair.

    d.  Accept default value for the security group.

    e.  Note that the wizard will create a role called “ecsInstanceRole”

    ![ECS Configure Cluster](images/ex0-image63.jpg)
    
7.  Click “Launch instance & run service”. The launch wizard will start the creation of several resources and this will take some time. When the allocation is complete, the “View Service” button will enable.

    ![Launch Status](images/ex0-image64.jpg)
    
8.  Click “View Service” when it becomes available. Then click on the running task ID.

    ![Task List](images/ex0-image65.jpg)
    
9.  Expand the disclosure triangle next to the “simple-app” container. Then click the External Link.

    ![Container IP](images/ex0-image66.jpg)
    
10. You should see a web page showing the sample app when the cluster is ready. It can take up to 30 minutes or more before your EC2 Container Service cluster is fully available.

    ![Sample Web App](images/ex0-image72.jpg)
    
    **Note: If you experience errors related to lack of available** **VMs, you may have to delete some other compute resources or request** **that Amazon raise the EC2 instance limit for your account. Changing to another region** **is a simple way around this limit, as the limit is assessed** **per region. Choose one of these options and try this again.**

## Task 11: Cleanup sample app

In this task, you will remove the sample app service. 
Simple though it is, this service is using resources you will need during the lab but you'll want to know how to remove them later.

1.  Navigate to the ECS home page, then choose the “fabmedical”.

    ![fabmedical cluster](images/ex0-image74.jpg)
    
2.  Check the checkmark next to the “simple-webapp” service. Then choose Update.

    ![Update Service Button](images/ex0-image75.jpg)

3.  Change the number of tasks to 0. Then click “Update Service”

    ![Update Service](images/ex0-image76.jpg)
    
4.  On the next screen click “View Service”. Refresh the deployment list until the running count reaches 0.

    ![Service Deployments](images/ex0-image77.jpg)
    
5.  When the Running count reaches 0, you can click “Delete”.

    ![Delete Service](images/ex0-image78.jpg)
    
6.  Next, confirm that you want to delete the service.

    You have deleted the service and receive the confirmation message as shown below.

    ![Service Deleted](images/ex0-image79.jpg)
