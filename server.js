const { response } = require('express');
const express = require('express');
const app = express(),
      bodyParser = require("body-parser");
      port = 3080;


const forbiddenStrings = ["cheese burger", "icecream", "sicily", "china", "space camp"]

const users = [];

app.use(bodyParser.json());
app.use(express.static(process.cwd()+"/my-app/dist/angular-nodejs-example/"));

app.get('/api/users', (req, res) => {
  res.json(users);
});

app.get('/api/redacted/:documentTitle', (req, res) => {
  //todo find the right document
  //todo redact it
  console.log("Got the message");
  console.log(req.params["documentTitle"]);
  var matchingDocument = users.filter(obj => {return obj.documentTitle === req.params["documentTitle"]})
  console.log("logging matchingDocument")
  console.log(matchingDocument);
  var text = JSON.parse(JSON.stringify(matchingDocument[0])).documentText;
  console.log("logging text before any operations")
  console.log(text);
  var i;
  for (i = 0; i < forbiddenStrings.length; i++)
  {
    console.log(forbiddenStrings[i]);
    const searchRegExp = new RegExp(forbiddenStrings[i], 'g');
    console.log(text);
    text = text.replace(searchRegExp, "XXXX");
  }
  var newDocument = {documentTitle: matchingDocument.documentTitle,
       documentText: text
  }
  
  res.json(newDocument);
})

app.post('/api/user', (req, res) => {
  const user = req.body.user;
  users.push(user);
  console.log(`added document ${user}`);
  console.log(user);
  res.json("user addedd");
});

app.get('/', (req,res) => {
  res.sendFile(process.cwd()+"/my-app/dist/angular-nodejs-example/index.html")
});

app.listen(port, () => {
    console.log(`Server listening on the port::${port}`);
});
