(function () {
    var app = angular.module("GSWS");
	app.controller("FarmerData", ["$scope", "$state", "PRRD_Services", '$sce', FarmerDataCall]);

    function FarmerDataCall(scope, state,PRRD_Services, sce) {

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }


        scope.detailsshow = false;
        var obj = {}
        PRRD_Services.POSTENCRYPTAPI("Project_Work_MasterData", obj, token, function (value) {
            var res = value.data;

            if (res.Status == "Success") {
                scope.Status = "Available";
                scope.Masterdata = res.data;
                //scope.detailsshow = true;
                //scope.detailsshowUID = false;
            }
            else if (res.Status == "428") {
                sessionStorage.clear();
                swal("info", "Session Expired !!!", "info");
                state.go("Login");
                return;
            }
            else {
                scope.Status = "Not Available";
                //scope.resdata = "";
                //scope.detailsshow = false;
                alert('No Data Found');
            }

        });



        scope.savedata = function () {


            //var obj = [];

            var input = {
                Item :
                    [{

                        FarmerId: scope.txtFarmerIDUID,
                        UID: scope.txtFarmerIDUID,
                        FarmerName: scope.FarmerName,
                        SurveyNo: scope.txtSurvey,
                        Extent: scope.txtExtent,
                        ProjectId: scope.ddlProject,
                        ProjectName: "",
                        WorkTypeCode: scope.ddlWork,
                        WorkName: "",
                        IsActive: null
                    }]
            };
           

            //obj.push(input);
                      
           
            

            PRRD_Services.POSTENCRYPTAPI("Save_FarmerData", input, token, function (value) {
                var res = value.data;

                if (res.Status == "Success") {
                    scope.Status = "Available";
                    alert(res.data[0].Remarks);
                    scope.detailsshow = false;
                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    scope.Status = "Not Available";
                    scope.detailsshow = true;
                    alert('No Data Found');
                }

            });
        };


        scope.getdetails = function () {

            if (scope.txtFarmerIDUID == undefined || scope.txtFarmerIDUID == null) {
                alert('Please enter Job Card Number.');
                return;
            }
            if (scope.ddlSearchby == undefined || scope.ddlSearchby == null) {
                alert('Please Select Search By.');
                return;
            }
            //{&quot;FarmerId&quot;:&quot;1&quot;,&quot;UID&quot;:&quot;&quot;,&quot;FarmerName&quot;:&quot;&quot;,&quot;Remarks&quot;:&quot;&quot;}
            var strWageseekerId = {
                FarmerId: scope.txtFarmerIDUID,
                UID: scope.txtFarmerIDUID,
                FarmerName: "",
                Remarks: "",
            };

            PRRD_Services.POSTENCRYPTAPI("Get_FarmerData", strWageseekerId, token, function (value) {
                var res = value.data;

                if (res.Status == "Success") {
                    scope.Status = "Available";
                    scope.FarmerName = res.data.FarmerName;
                    //scope.detailsshow = true;
                    //scope.detailsshowUID = false;
                    scope.detailsshow = true;
                }
                else if (res.Status == "428") {
                    sessionStorage.clear();
                    swal("info", "Session Expired !!!", "info");
                    state.go("Login");
                    return;
                }
                else {
                    scope.Status = "Not Available";
                    //scope.resdata = "";
                    //scope.detailsshow = true;
                    alert('No Data Found');
                }

            });
        };
    }

    app.filter('unique', function () {
        return function (collection, keyname) {
            var output = [],
                keys = []
            found = [];

            if (!keyname) {

                angular.forEach(collection, function (row) {
                    var is_found = false;
                    angular.forEach(found, function (foundRow) {

                        if (foundRow == row) {
                            is_found = true;
                        }
                    });

                    if (is_found) { return; }
                    found.push(row);
                    output.push(row);

                });
            }
            else {

                angular.forEach(collection, function (row) {
                    var item = row[keyname];
                    if (item === null || item === undefined) return;
                    if (keys.indexOf(item) === -1) {
                        keys.push(item);
                        output.push(row);
                    }
                });
            }

            return output;
        };
    });
})();