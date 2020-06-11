(function () {
    var app = angular.module("GSWS");

    app.controller("TestMainController", ["$scope", "$state", "$log", "Login_Services", "$filter", Main_CTRL]);

    function Main_CTRL(scope, state, log, Login_Services, $filter) {


        var username = "TEST";
        scope.username = username;
        scope.ldate = new Date().toLocaleDateString();


        var input = { Ftype: 1}

        Login_Services.POSTAPI("GetAllURLList", input, function (value) {
            var res = value.data;
            if (res.Status == "100") {

                scope.main_data = res.DataList;
               // scope.main_data = url_data;
                scope.url_data = scope.main_data;
                scope.final_data = [];
                scope.hods = [];
                loadalladata();
            }
            else {
                swal('info', 'Invalid Request', 'info');
            }


        });
        

        scope.process_url = function (obj) {

            gettransIntitate(obj)
        };

        function gettransIntitate(obj) {

            scope.distcode = localStorage.getItem("distcode");
            scope.mcode = localStorage.getItem("mcode");
            scope.gpcode= localStorage.getItem("gpcode");
            scope.secratariat_id=localStorage.getItem("secccode");
            var username = localStorage.getItem("user");
            var token = sessionStorage.getItem("Token");
            var input = { DEPT_ID: obj.SD_ID, HOD_ID: obj.HOD_ID, SERVICE_ID: obj.SD_ID, DISTRICT_ID: scope.distcode, MANDAL_ID: scope.mcode, GP_WARD_ID: scope.gpcode, LOGIN_USER: username, TYPE_OF_REQUEST: obj.TYPE_OF_SERVICE, URL_ID: obj.URL_ID, SECRETRAINT_CODE: scope.secratariat_id};

            Login_Services.POSTTRANSCRYPTAPI("initiateTransaction", input, token, function (value) {
                var res = value.data;
                if (res.status == "200") {

                    scope.translist = res.Translist;

                    if (obj.TYPE_OF_SERVICE == "1") {
                        var input = { USERNAME: username, PASSWORD: username + 1, PS_TXN_ID: res.Translist[0].TNSId, RETURN_URL: "http://prajasachivalayam.ap.gov.in/PSTESTAPP/#!/Login" }
                        Login_Services.POSTENCRYPTAPI("GetEncryptthirdparty", input, token, function (value) {
                            var res = value.data;
                            if (res.Status == "100") {

                                var url = obj.URL+"?Id=" + res.encrypttext + "^" + res.key;
                                window.open(url, '_blank');
                                //window.open(url, "", "scrollbars=yes,resizable=yes,top=50,left=200,width=1000,height=500");
                                //	window.open( "_blank");	
                                return;
                            }
                            else {
                                swal('info', 'Invalid Request', 'info');
                            }


                        });
                    }
                    else {
                        window.open(obj.URL,"_blank");	
                    }
                    //window.open(url, "", "scrollbars=yes,resizable=yes,top=50,left=200,width=1000,height=500");
                    //	
                    return;
                }
                else {
                    swal('info', 'Invalid Request', 'info');
                }


            });
        }

        scope.dept_checker = function (dept_id) {
            for (var i = 0; i < scope.final_data.length; i++) {
                if (scope.final_data[i].DEPT_ID == dept_id) {
                    return false;
                }
            }
            return true;
        };

        scope.hod_checker = function (hod_id) {
            for (var i = 0; i < scope.hods.length; i++) {
                if (scope.hods[i].HOD_ID == hod_id) {
                    return false;
                }
            }
            return true;
        };

        function loadalladata() {
            for (var i = 0; i < scope.url_data.length; i++) {
                if (scope.dept_checker(scope.url_data[i].SD_ID)) {
                    scope.final_data.push({
                        DEPT_NAME: scope.url_data[i].SD_NAME,
                        DEPT_ID: scope.url_data[i].SD_ID,
                        HOD_LIST: null
                    });
                }
            }

            for (var i = 0; i < scope.url_data.length; i++) {
                if (scope.hod_checker(scope.url_data[i].HOD_ID)) {
                    var temp_data = {
                        HOD_ID: scope.url_data[i].HOD_ID,
                        HOD_NAME: scope.url_data[i].HOD_NAME,
                        DEPT_NAME: scope.url_data[i].SD_NAME,
                        DEPT_ID: scope.url_data[i].SD_ID
                    };
                    scope.hods.push(temp_data);
                }
            }

            scope.dept_count = 0;
            scope.HOD_LIST = [];
            for (var k = 0; k < scope.hods.length; k++) {
                scope.url_list = [];

                for (var i = 0; i < scope.url_data.length; i++) {
                    if (scope.hods[k].HOD_ID == scope.url_data[i].HOD_ID) {
                        scope.url_list.push({

                            SD_ID: scope.url_data[i].SD_ID,
                            SD_NAME: scope.url_data[i].SD_NAME,
                            HOD_ID: scope.url_data[i].HOD_ID,
                            HOD_NAME: scope.url_data[i].HOD_NAME,
                            URL_ID: scope.url_data[i].URL_ID,
                            URL: scope.url_data[i].URL,
                            URL_DESCRIPTION: scope.url_data[i].URL_DESCRIPTION,
                            TYPE_OF_SERVICE: scope.url_data[i].TYPE_OF_SERVICE

                        });
                    }
                }

                scope.HOD_LIST.push(
                    {
                        DEPT_NAME: scope.hods[k].DEPT_NAME,
                        DEPT_ID: scope.hods[k].DEPT_ID,
                        HOD_ID: scope.hods[k].HOD_ID,
                        HOD_NAME: scope.hods[k].HOD_NAME,
                        URL_LIST: scope.url_list
                    });
            }

            for (var i = 0; i < scope.final_data.length; i++) {
                scope.temp_hod_list = [];
                for (var j = 0; j < scope.HOD_LIST.length; j++) {
                    if (scope.final_data[i].DEPT_ID == scope.HOD_LIST[j].DEPT_ID) {
                        scope.temp_hod_list.push(scope.HOD_LIST[j]);
                    }
                }

                scope.final_data[scope.dept_count].HOD_LIST = scope.temp_hod_list;
                scope.dept_count += 1;

            }

            scope.result = scope.final_data;

        }
        scope.get_data = function () {

            if (scope.search.URL_DESCRIPTION == "") {
                scope.viewDropdown = false;
            }
            else {
                scope.viewDropdown = true;
            }

            scope.url_data = $filter('filter')(scope.main_data, { URL_DESCRIPTION: scope.search.URL_DESCRIPTION });


            scope.final_data = [];
            scope.hods = [];

            for (var i = 0; i < scope.url_data.length; i++) {
                if (scope.dept_checker(scope.url_data[i].SD_ID)) {
                    scope.final_data.push({
                        DEPT_NAME: scope.url_data[i].SD_NAME,
                        DEPT_ID: scope.url_data[i].SD_ID,
                        HOD_LIST: null
                    });
                }
            }

            for (var i = 0; i < scope.url_data.length; i++) {
                if (scope.hod_checker(scope.url_data[i].HOD_ID)) {
                    var temp_data = {
                        HOD_ID: scope.url_data[i].HOD_ID,
                        HOD_NAME: scope.url_data[i].HOD_NAME,
                        DEPT_NAME: scope.url_data[i].SD_NAME,
                        DEPT_ID: scope.url_data[i].SD_ID
                    };
                    scope.hods.push(temp_data);
                }
            }

            scope.dept_count = 0;
            scope.HOD_LIST = [];
            for (var k = 0; k < scope.hods.length; k++) {
                scope.url_list = [];

                for (var i = 0; i < scope.url_data.length; i++) {
                    if (scope.hods[k].HOD_ID == scope.url_data[i].HOD_ID) {
                        scope.url_list.push({

                            SD_ID: scope.url_data[i].SD_ID,
                            SD_NAME: scope.url_data[i].SD_NAME,
                            HOD_ID: scope.url_data[i].HOD_ID,
                            HOD_NAME: scope.url_data[i].HOD_NAME,
                            URL_ID: scope.url_data[i].URL_ID,
                            URL: scope.url_data[i].URL,
                            URL_DESCRIPTION: scope.url_data[i].URL_DESCRIPTION,
                            TYPE_OF_SERVICE: scope.url_data[i].TYPE_OF_SERVICE

                        });
                    }
                }

                scope.HOD_LIST.push(
                    {
                        DEPT_NAME: scope.hods[k].DEPT_NAME,
                        DEPT_ID: scope.hods[k].DEPT_ID,
                        HOD_ID: scope.hods[k].HOD_ID,
                        HOD_NAME: scope.hods[k].HOD_NAME,
                        URL_LIST: scope.url_list
                    });
            }



            for (var i = 0; i < scope.final_data.length; i++) {
                scope.temp_hod_list = [];
                for (var j = 0; j < scope.HOD_LIST.length; j++) {
                    if (scope.final_data[i].DEPT_ID == scope.HOD_LIST[j].DEPT_ID) {
                        scope.temp_hod_list.push(scope.HOD_LIST[j]);
                    }
                }

                scope.final_data[scope.dept_count].HOD_LIST = scope.temp_hod_list;
                scope.dept_count += 1;

            }


        };

        scope.onMouseHold = function (e, obj) {
            scope.hint_frame = true;
            e = e || window.event; //window.event for IE
            // alert("Keycode of key pressed: " + (e.keyCode || e.which));
            // alert(obj);

        };
        scope.onMouseRelease = function () {
            scope.hint_frame = false;
        };

        scope.GetLogout = function () {
            localStorage.clear();
            sessionStorage.clear();
            state.go("Login");
        };

    }




})();

