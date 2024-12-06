# AugmentationsAPI

## Introduction
This is a **C# Web API** University Project themed around the numerous *Augmentations* featured in the *Deus Ex* game franchise.

## Usage
### Getting Started
* Clone the repo in Visual Studio

* Open the Package Manager Console from **View > Other Windows > Package Manager Console**

* Run the following command in the **Package Manager Console** to create the Local Database: 
```powershell
Update-Database
```
* *Build* and *Run* the *Project*, then navigate to the *Local Host Port* where you can find the *Swagger UI*
> [!WARNING]  
> Before you can make any other Requests to the API you need to *Register* and *Login* to recieve a *JWT Token*

### Registering and Logging in
* Navigate to **Post/Identity/Register** and Click on It
* Click **Try it out** and then Click **Execute** to Execute the API Call with the Example Data
* Navigate to **Post/Identity/Login** and Click on It
* Click **Try it out** and then Click **Execute** to Log in to your previously created Account
* Copy the **Token** in the Response Body of Executed API Call
* Navigate to the **Top** of the Swagger UI and Click the **Authorize** Button
* Pass your **Token** to the *Value Field* of the *Pop Up Window* in the **following format**:
  ```powershell
  bearer yourTokenHere
  ```
### Now you are Ready to Make Requests to the Augmentations Section of the API!
