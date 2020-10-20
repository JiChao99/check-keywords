using NUnit.Framework;
using Shared.Handlers;
using System.Linq;

namespace Test.JsonHandlerTests
{
    public class BuildNeedCheckWordsTest
    {
        private JsonHandler _jsonHandler;
        private SwaggerHandler _swaggerHandler;

        [SetUp]
        public void SetUp()
        {
            _jsonHandler = new JsonHandler();
            _swaggerHandler = new SwaggerHandler();
        }

        [Test]
        public void InputEmptyObjectReturnEmptyList()
        {
            var result = _jsonHandler.BuildNeedCheckWords("{}");

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void InputOnlyOneArrayReturnEmptyList()
        {
            var result = _jsonHandler.BuildNeedCheckWords("[]");

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void InputOneObjectReturnOneItem()
        {
            const string KEY = "function";

            var result = _jsonHandler.BuildNeedCheckWords($"{{\"{KEY}\":\"\"}}");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(KEY, result.Single());
        }

        [Test]
        public void InputNestedObjects()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{ \"{KEY_2}\": {{ \"{KEY_3}\": {{}} }} }} }}");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
        }

        [Test]
        public void InputObjectOnTheSameLevel()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{}}, \"{KEY_2}\": {{}}, \"{KEY_3}\": {{}} }}");

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
        }

        [Test]
        public void InputObjectOnTheSameLevelAndNestedObjects()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";
            const string KEY_4 = "b";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{ \"{KEY_2}\": {{}}, \"{KEY_3}\": {{}} }}, \"{KEY_4}\": {{}} }}");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
            Assert.AreEqual(KEY_4, result[3]);
        }

        [Test]
        public void InputObjectOnTheSameLevelAndNestedObjectsAndArray()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";
            const string KEY_4 = "b";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{ \"{KEY_2}\": [], \"{KEY_3}\": {{}} }}, \"{KEY_4}\": [] }}");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
            Assert.AreEqual(KEY_4, result[3]);
        }

        [Test]
        public void InputEverything()
        {
            const string KEY_1 = "function";
            const string KEY_2 = "action";
            const string KEY_3 = "sub";
            const string KEY_4 = "b";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY_1}\": {{ \"{KEY_2}\": null, \"{KEY_3}\": 123 }}, \"{KEY_4}\": [] }}");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(KEY_1, result[0]);
            Assert.AreEqual(KEY_2, result[1]);
            Assert.AreEqual(KEY_3, result[2]);
            Assert.AreEqual(KEY_4, result[3]);
        }

        [Test]
        public void InputDuplicateKey()
        {
            const string KEY = "function";

            var result = _jsonHandler.BuildNeedCheckWords($"{{ \"{KEY}\": {{ \"{KEY}\": null, \"{KEY}\": 123 }}, \"{KEY}\": [] }}");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(KEY, result.Single());
        }

        [Test]
        public void InputSwaggerJson()
        {
            var wordsStr = "body,status,tags,petId,name,api_key,additionalMetadata,file,orderId,username,password,id,quantity,shipDate,complete,firstName,lastName,email,phone,userStatus,category,photoUrls,code,type,message";
            var wordsList = wordsStr.Split(",").ToList();
            var swaggerJson = "{\"swagger\":\"2.0\",\"info\":{\"description\":\"ThisisasampleserverPetstoreserver.YoucanfindoutmoreaboutSwaggerat[http://swagger.io](http://swagger.io)oron[irc.freenode.net,#swagger](http://swagger.io/irc/).Forthissample,youcanusetheapikey`special-key`totesttheauthorizationfilters.\",\"version\":\"1.0.0\",\"title\":\"SwaggerPetstore\",\"termsOfService\":\"http://swagger.io/terms/\",\"contact\":{\"email\":\"apiteam@swagger.io\"},\"license\":{\"name\":\"Apache2.0\",\"url\":\"http://www.apache.org/licenses/LICENSE-2.0.html\"}},\"host\":\"petstore.swagger.io\",\"basePath\":\"/v2\",\"tags\":[{\"name\":\"pet\",\"description\":\"EverythingaboutyourPets\",\"externalDocs\":{\"description\":\"Findoutmore\",\"url\":\"http://swagger.io\"}},{\"name\":\"store\",\"description\":\"AccesstoPetstoreorders\"},{\"name\":\"user\",\"description\":\"Operationsaboutuser\",\"externalDocs\":{\"description\":\"Findoutmoreaboutourstore\",\"url\":\"http://swagger.io\"}}],\"schemes\":[\"https\",\"http\"],\"paths\":{\"/pet\":{\"post\":{\"tags\":[\"pet\"],\"summary\":\"Addanewpettothestore\",\"description\":\"\",\"operationId\":\"addPet\",\"consumes\":[\"application/json\",\"application/xml\"],\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"in\":\"body\",\"name\":\"body\",\"description\":\"Petobjectthatneedstobeaddedtothestore\",\"required\":true,\"schema\":{\"$ref\":\"#/definitions/Pet\"}}],\"responses\":{\"405\":{\"description\":\"Invalidinput\"}},\"security\":[{\"petstore_auth\":[\"write:pets\",\"read:pets\"]}]},\"put\":{\"tags\":[\"pet\"],\"summary\":\"Updateanexistingpet\",\"description\":\"\",\"operationId\":\"updatePet\",\"consumes\":[\"application/json\",\"application/xml\"],\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"in\":\"body\",\"name\":\"body\",\"description\":\"Petobjectthatneedstobeaddedtothestore\",\"required\":true,\"schema\":{\"$ref\":\"#/definitions/Pet\"}}],\"responses\":{\"400\":{\"description\":\"InvalidIDsupplied\"},\"404\":{\"description\":\"Petnotfound\"},\"405\":{\"description\":\"Validationexception\"}},\"security\":[{\"petstore_auth\":[\"write:pets\",\"read:pets\"]}]}},\"/pet/findByStatus\":{\"get\":{\"tags\":[\"pet\"],\"summary\":\"FindsPetsbystatus\",\"description\":\"Multiplestatusvaluescanbeprovidedwithcommaseparatedstrings\",\"operationId\":\"findPetsByStatus\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"status\",\"in\":\"query\",\"description\":\"Statusvaluesthatneedtobeconsideredforfilter\",\"required\":true,\"type\":\"array\",\"items\":{\"type\":\"string\",\"enum\":[\"available\",\"pending\",\"sold\"],\"default\":\"available\"},\"collectionFormat\":\"multi\"}],\"responses\":{\"200\":{\"description\":\"successfuloperation\",\"schema\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/Pet\"}}},\"400\":{\"description\":\"Invalidstatusvalue\"}},\"security\":[{\"petstore_auth\":[\"write:pets\",\"read:pets\"]}]}},\"/pet/findByTags\":{\"get\":{\"tags\":[\"pet\"],\"summary\":\"FindsPetsbytags\",\"description\":\"Mulipletagscanbeprovidedwithcommaseparatedstrings.Usetag1,tag2,tag3fortesting.\",\"operationId\":\"findPetsByTags\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"tags\",\"in\":\"query\",\"description\":\"Tagstofilterby\",\"required\":true,\"type\":\"array\",\"items\":{\"type\":\"string\"},\"collectionFormat\":\"multi\"}],\"responses\":{\"200\":{\"description\":\"successfuloperation\",\"schema\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/Pet\"}}},\"400\":{\"description\":\"Invalidtagvalue\"}},\"security\":[{\"petstore_auth\":[\"write:pets\",\"read:pets\"]}],\"deprecated\":true}},\"/pet/{petId}\":{\"get\":{\"tags\":[\"pet\"],\"summary\":\"FindpetbyID\",\"description\":\"Returnsasinglepet\",\"operationId\":\"getPetById\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"petId\",\"in\":\"path\",\"description\":\"IDofpettoreturn\",\"required\":true,\"type\":\"integer\",\"format\":\"int64\"}],\"responses\":{\"200\":{\"description\":\"successfuloperation\",\"schema\":{\"$ref\":\"#/definitions/Pet\"}},\"400\":{\"description\":\"InvalidIDsupplied\"},\"404\":{\"description\":\"Petnotfound\"}},\"security\":[{\"api_key\":[]}]},\"post\":{\"tags\":[\"pet\"],\"summary\":\"Updatesapetinthestorewithformdata\",\"description\":\"\",\"operationId\":\"updatePetWithForm\",\"consumes\":[\"application/x-www-form-urlencoded\"],\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"petId\",\"in\":\"path\",\"description\":\"IDofpetthatneedstobeupdated\",\"required\":true,\"type\":\"integer\",\"format\":\"int64\"},{\"name\":\"name\",\"in\":\"formData\",\"description\":\"Updatednameofthepet\",\"required\":false,\"type\":\"string\"},{\"name\":\"status\",\"in\":\"formData\",\"description\":\"Updatedstatusofthepet\",\"required\":false,\"type\":\"string\"}],\"responses\":{\"405\":{\"description\":\"Invalidinput\"}},\"security\":[{\"petstore_auth\":[\"write:pets\",\"read:pets\"]}]},\"delete\":{\"tags\":[\"pet\"],\"summary\":\"Deletesapet\",\"description\":\"\",\"operationId\":\"deletePet\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"api_key\",\"in\":\"header\",\"required\":false,\"type\":\"string\"},{\"name\":\"petId\",\"in\":\"path\",\"description\":\"Petidtodelete\",\"required\":true,\"type\":\"integer\",\"format\":\"int64\"}],\"responses\":{\"400\":{\"description\":\"InvalidIDsupplied\"},\"404\":{\"description\":\"Petnotfound\"}},\"security\":[{\"petstore_auth\":[\"write:pets\",\"read:pets\"]}]}},\"/pet/{petId}/uploadImage\":{\"post\":{\"tags\":[\"pet\"],\"summary\":\"uploadsanimage\",\"description\":\"\",\"operationId\":\"uploadFile\",\"consumes\":[\"multipart/form-data\"],\"produces\":[\"application/json\"],\"parameters\":[{\"name\":\"petId\",\"in\":\"path\",\"description\":\"IDofpettoupdate\",\"required\":true,\"type\":\"integer\",\"format\":\"int64\"},{\"name\":\"additionalMetadata\",\"in\":\"formData\",\"description\":\"Additionaldatatopasstoserver\",\"required\":false,\"type\":\"string\"},{\"name\":\"file\",\"in\":\"formData\",\"description\":\"filetoupload\",\"required\":false,\"type\":\"file\"}],\"responses\":{\"200\":{\"description\":\"successfuloperation\",\"schema\":{\"$ref\":\"#/definitions/ApiResponse\"}}},\"security\":[{\"petstore_auth\":[\"write:pets\",\"read:pets\"]}]}},\"/store/inventory\":{\"get\":{\"tags\":[\"store\"],\"summary\":\"Returnspetinventoriesbystatus\",\"description\":\"Returnsamapofstatuscodestoquantities\",\"operationId\":\"getInventory\",\"produces\":[\"application/json\"],\"parameters\":[],\"responses\":{\"200\":{\"description\":\"successfuloperation\",\"schema\":{\"type\":\"object\",\"additionalProperties\":{\"type\":\"integer\",\"format\":\"int32\"}}}},\"security\":[{\"api_key\":[]}]}},\"/store/order\":{\"post\":{\"tags\":[\"store\"],\"summary\":\"Placeanorderforapet\",\"description\":\"\",\"operationId\":\"placeOrder\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"in\":\"body\",\"name\":\"body\",\"description\":\"orderplacedforpurchasingthepet\",\"required\":true,\"schema\":{\"$ref\":\"#/definitions/Order\"}}],\"responses\":{\"200\":{\"description\":\"successfuloperation\",\"schema\":{\"$ref\":\"#/definitions/Order\"}},\"400\":{\"description\":\"InvalidOrder\"}}}},\"/store/order/{orderId}\":{\"get\":{\"tags\":[\"store\"],\"summary\":\"FindpurchaseorderbyID\",\"description\":\"ForvalidresponsetryintegerIDswithvalue>=1and<=10.Othervalueswillgeneratedexceptions\",\"operationId\":\"getOrderById\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"orderId\",\"in\":\"path\",\"description\":\"IDofpetthatneedstobefetched\",\"required\":true,\"type\":\"integer\",\"maximum\":10,\"minimum\":1,\"format\":\"int64\"}],\"responses\":{\"200\":{\"description\":\"successfuloperation\",\"schema\":{\"$ref\":\"#/definitions/Order\"}},\"400\":{\"description\":\"InvalidIDsupplied\"},\"404\":{\"description\":\"Ordernotfound\"}}},\"delete\":{\"tags\":[\"store\"],\"summary\":\"DeletepurchaseorderbyID\",\"description\":\"ForvalidresponsetryintegerIDswithpositiveintegervalue.Negativeornon-integervalueswillgenerateAPIerrors\",\"operationId\":\"deleteOrder\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"orderId\",\"in\":\"path\",\"description\":\"IDoftheorderthatneedstobedeleted\",\"required\":true,\"type\":\"integer\",\"minimum\":1,\"format\":\"int64\"}],\"responses\":{\"400\":{\"description\":\"InvalidIDsupplied\"},\"404\":{\"description\":\"Ordernotfound\"}}}},\"/user\":{\"post\":{\"tags\":[\"user\"],\"summary\":\"Createuser\",\"description\":\"Thiscanonlybedonebytheloggedinuser.\",\"operationId\":\"createUser\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"in\":\"body\",\"name\":\"body\",\"description\":\"Createduserobject\",\"required\":true,\"schema\":{\"$ref\":\"#/definitions/User\"}}],\"responses\":{\"default\":{\"description\":\"successfuloperation\"}}}},\"/user/createWithArray\":{\"post\":{\"tags\":[\"user\"],\"summary\":\"Createslistofuserswithgiveninputarray\",\"description\":\"\",\"operationId\":\"createUsersWithArrayInput\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"in\":\"body\",\"name\":\"body\",\"description\":\"Listofuserobject\",\"required\":true,\"schema\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/User\"}}}],\"responses\":{\"default\":{\"description\":\"successfuloperation\"}}}},\"/user/createWithList\":{\"post\":{\"tags\":[\"user\"],\"summary\":\"Createslistofuserswithgiveninputarray\",\"description\":\"\",\"operationId\":\"createUsersWithListInput\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"in\":\"body\",\"name\":\"body\",\"description\":\"Listofuserobject\",\"required\":true,\"schema\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/User\"}}}],\"responses\":{\"default\":{\"description\":\"successfuloperation\"}}}},\"/user/login\":{\"get\":{\"tags\":[\"user\"],\"summary\":\"Logsuserintothesystem\",\"description\":\"\",\"operationId\":\"loginUser\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"username\",\"in\":\"query\",\"description\":\"Theusernameforlogin\",\"required\":true,\"type\":\"string\"},{\"name\":\"password\",\"in\":\"query\",\"description\":\"Thepasswordforloginincleartext\",\"required\":true,\"type\":\"string\"}],\"responses\":{\"200\":{\"description\":\"successfuloperation\",\"schema\":{\"type\":\"string\"},\"headers\":{\"X-Rate-Limit\":{\"type\":\"integer\",\"format\":\"int32\",\"description\":\"callsperhourallowedbytheuser\"},\"X-Expires-After\":{\"type\":\"string\",\"format\":\"date-time\",\"description\":\"dateinUTCwhentokenexpires\"}}},\"400\":{\"description\":\"Invalidusername/passwordsupplied\"}}}},\"/user/logout\":{\"get\":{\"tags\":[\"user\"],\"summary\":\"Logsoutcurrentloggedinusersession\",\"description\":\"\",\"operationId\":\"logoutUser\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[],\"responses\":{\"default\":{\"description\":\"successfuloperation\"}}}},\"/user/{username}\":{\"get\":{\"tags\":[\"user\"],\"summary\":\"Getuserbyusername\",\"description\":\"\",\"operationId\":\"getUserByName\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"username\",\"in\":\"path\",\"description\":\"Thenamethatneedstobefetched.Useuser1fortesting.\",\"required\":true,\"type\":\"string\"}],\"responses\":{\"200\":{\"description\":\"successfuloperation\",\"schema\":{\"$ref\":\"#/definitions/User\"}},\"400\":{\"description\":\"Invalidusernamesupplied\"},\"404\":{\"description\":\"Usernotfound\"}}},\"put\":{\"tags\":[\"user\"],\"summary\":\"Updateduser\",\"description\":\"Thiscanonlybedonebytheloggedinuser.\",\"operationId\":\"updateUser\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"username\",\"in\":\"path\",\"description\":\"namethatneedtobeupdated\",\"required\":true,\"type\":\"string\"},{\"in\":\"body\",\"name\":\"body\",\"description\":\"Updateduserobject\",\"required\":true,\"schema\":{\"$ref\":\"#/definitions/User\"}}],\"responses\":{\"400\":{\"description\":\"Invalidusersupplied\"},\"404\":{\"description\":\"Usernotfound\"}}},\"delete\":{\"tags\":[\"user\"],\"summary\":\"Deleteuser\",\"description\":\"Thiscanonlybedonebytheloggedinuser.\",\"operationId\":\"deleteUser\",\"produces\":[\"application/xml\",\"application/json\"],\"parameters\":[{\"name\":\"username\",\"in\":\"path\",\"description\":\"Thenamethatneedstobedeleted\",\"required\":true,\"type\":\"string\"}],\"responses\":{\"400\":{\"description\":\"Invalidusernamesupplied\"},\"404\":{\"description\":\"Usernotfound\"}}}}},\"securityDefinitions\":{\"petstore_auth\":{\"type\":\"oauth2\",\"authorizationUrl\":\"http://petstore.swagger.io/oauth/dialog\",\"flow\":\"implicit\",\"scopes\":{\"write:pets\":\"modifypetsinyouraccount\",\"read:pets\":\"readyourpets\"}},\"api_key\":{\"type\":\"apiKey\",\"name\":\"api_key\",\"in\":\"header\"}},\"definitions\":{\"Order\":{\"type\":\"object\",\"properties\":{\"id\":{\"type\":\"integer\",\"format\":\"int64\"},\"petId\":{\"type\":\"integer\",\"format\":\"int64\"},\"quantity\":{\"type\":\"integer\",\"format\":\"int32\"},\"shipDate\":{\"type\":\"string\",\"format\":\"date-time\"},\"status\":{\"type\":\"string\",\"description\":\"OrderStatus\",\"enum\":[\"placed\",\"approved\",\"delivered\"]},\"complete\":{\"type\":\"boolean\",\"default\":false}},\"xml\":{\"name\":\"Order\"}},\"Category\":{\"type\":\"object\",\"properties\":{\"id\":{\"type\":\"integer\",\"format\":\"int64\"},\"name\":{\"type\":\"string\"}},\"xml\":{\"name\":\"Category\"}},\"User\":{\"type\":\"object\",\"properties\":{\"id\":{\"type\":\"integer\",\"format\":\"int64\"},\"username\":{\"type\":\"string\"},\"firstName\":{\"type\":\"string\"},\"lastName\":{\"type\":\"string\"},\"email\":{\"type\":\"string\"},\"password\":{\"type\":\"string\"},\"phone\":{\"type\":\"string\"},\"userStatus\":{\"type\":\"integer\",\"format\":\"int32\",\"description\":\"UserStatus\"}},\"xml\":{\"name\":\"User\"}},\"Tag\":{\"type\":\"object\",\"properties\":{\"id\":{\"type\":\"integer\",\"format\":\"int64\"},\"name\":{\"type\":\"string\"}},\"xml\":{\"name\":\"Tag\"}},\"Pet\":{\"type\":\"object\",\"required\":[\"name\",\"photoUrls\"],\"properties\":{\"id\":{\"type\":\"integer\",\"format\":\"int64\"},\"category\":{\"$ref\":\"#/definitions/Category\"},\"name\":{\"type\":\"string\",\"example\":\"doggie\"},\"photoUrls\":{\"type\":\"array\",\"xml\":{\"name\":\"photoUrl\",\"wrapped\":true},\"items\":{\"type\":\"string\"}},\"tags\":{\"type\":\"array\",\"xml\":{\"name\":\"tag\",\"wrapped\":true},\"items\":{\"$ref\":\"#/definitions/Tag\"}},\"status\":{\"type\":\"string\",\"description\":\"petstatusinthestore\",\"enum\":[\"available\",\"pending\",\"sold\"]}},\"xml\":{\"name\":\"Pet\"}},\"ApiResponse\":{\"type\":\"object\",\"properties\":{\"code\":{\"type\":\"integer\",\"format\":\"int32\"},\"type\":{\"type\":\"string\"},\"message\":{\"type\":\"string\"}}}},\"externalDocs\":{\"description\":\"FindoutmoreaboutSwagger\",\"url\":\"http://swagger.io\"}}";
            
            var result = _swaggerHandler.BuildNeedCheckWords(swaggerJson);

            Assert.AreEqual(wordsList.Count, result.Count);
            Assert.IsTrue(wordsList.All(t => result.Contains(t)));
        }
    }
}
