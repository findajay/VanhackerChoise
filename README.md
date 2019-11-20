# Vanhack Engine
<img src="https://m.media-amazon.com/images/G/01/mobile-apps/dex/alexa/alexa-skills-kit/tutorials/quiz-game/header._TTH_.png" />

## Invocation : 'Alexa, start vanhack engine.' 
 
This is an alexa skill, which will help vanhack recruiters in searching for best match candidates and candidates can look up for latest available jobs according to skill set.

This Alexa Vanhack (VanhackerChoise/VanhackRecruit.json) skill is a template for a skill development. Provided a list of available candidates based on the provided filters, Alexa will select a candidate based on searching happening in backend APIs and tell it to the user when the skill is invoked.

## Skill Architecture
Each skill consists of two basic parts, a front end and a back end. The front end is the voice interface, or VUI. The voice interface is configured through the voice interaction model. The back end is where the logic of your skill resides.

UI - It can be used on any Alexa device (If you don't have one no worry, you can use https://echosim.io/welcome)
MiddleWare - Microsoft logic app for recieving and sending responses to alexa.
Bakend Server - For all processing on data and filteration model ,I have used function app to make it system complete server less.
Data files - All data csv files are stored on blob and data point URLs can be configured in function app configuration setting.

## Three Steps for Skill End to End Setup: 
Please follow thses steps to setup Vanhack skill with your amazon account.

1. Download "VanhackerChoise/VanhackRecruit.json" file from development branch.
2. Go to https://developer.amazon.com/alexa/console/ask
3. Login with you amazon account or sign up for an account
4. Click on create a skill
5. Name your skill for e.g. Vanhack
6. Choose a template (start from scratch)
7. Now go to json editor in left panel menu and paste the json or drag and drop Alexa skill downloaded from here.
8. Click save model on top and you have your skill setup done.

Now that you have skills read, It requires an https webhook endpoint to send input and recieve response.
9. Deploy your logic app using json file VanhackerChoise/SkillSelectLogicApp/SkillSelect.json on azure.
10.Go back to your Alexa skill console and click Endpoint in left menu than select https and put URL of logic app. 

## My Logic app host
https://prod-22.centralus.logic.azure.com:443/workflows/d0ff9f44974e4f38b9cd5694cd42511e/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=r2u7ePAPch6yNcm73HbfFv6GDz2s2aAEMzHojNRIfU8

Now Alexa can welcome you to the Engine but would not perform further action because backend processing unit is not deployed.

11. Downlaod source code for function app from location "VanhackerChoise/VanhackRecruitAPI".
12. Open it in visual studio and publish it using your azure subscription resources or Use azure portal to do that. (This can be easily handed by CI/CD in future)
13. Confirm these two settngs from function configuration 
  {
    "name": "AvailableCandidatesBlob",
    "value": "https://skillselectstore.blob.core.windows.net/skillselectcontainer/AvailableCandidates.csv",
    "slotSetting": false
  },
  {
    "name": "AvailableJobsBlob",
    "value": "https://skillselectstore.blob.core.windows.net/skillselectcontainer/JobsToPredict.csv",
    "slotSetting": false
  }
 14. Now we are ready to go for search.
 15. Start it using invocation speech and follow along.
 
