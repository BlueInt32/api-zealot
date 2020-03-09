# Api Zealot - Design questions

## Who are we designing for?

Api zealot is a tool for developers and QA ingeneers mostly. Every person who is likely to call and test an API is being targeted by Api Zealot. People with more ops background could be interested too if usability is good enough, e.g. if you could paste a curl command into the app.

I am not sure this tool would target people who do not know the ABC of programming. Maybe. For now I think it is better that asserting is done only by JS code. 

## What are the problems that they face? 

Those people face two different problems : 

- ### Scraping a draft request

  They sometimes need to query a single API endpoint quickly, maybe tweek a param or two, see the response and then leave without saving anything. There is a thing about urgency, usability is key at that moment because when you are in the middle of something else, you do not want the api tool to get in the way. But at some point, they may think this draft should be included in a more robust scenario.

- ### Building and saving a testing scenario

  They sometimes need to build a more or less complex testing scenario : calling several API, asserting things on their responses, wait for 10s, loop. Maybe they need to create a dataset before that. They may need to execute tests on different environments. The main thing here is that they need to be able to build their scenario in the most efficient and reliable way possible. This process consists in a series of trial and error calls to the tested API. Finally they need to be able to persist their tests and version it just as if they were saving any text file in Notepad. 

## How can we go about solving those problems?



## Why might our solution not work? 

## What can we do about that? 

## Why was a decision made by our team or company? 