(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("MUIDAppStatus", ["$scope", "$state", "$log", "Ser_Services", Services_CTRL]);

    function Services_CTRL(scope, state, log, s_services) {
        scope.pagename = "Housing Sites";
        scope.preloader = false;
        scope.dttable = false;
        var type = GetParameterValues('type');
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        var uldArray = ["kurnool", "guntakal", "dharmavaram", "kalyanadurgam", "gooty", "tadipatri", "puttaparthy", "pamidi", "kadiri", "rayadurg", "madakasira", "hindupur", "anantapur", "palamaner", "srikalahasti", "nagari", "madanapalle", "punganur", "tirupati", "puttur", "chittoor", "mydukur", "proddatur", "rayachoty", "kadapa", "pulivendula", "jammalamadugu", "rajampet", "yerraguntla", "budwel", "ramachandrapuram", "amalapuram", "rajahmundry", "kakinada", "yeleswaram", "tuni", "mummidivaram", "gollaprolu", "peddapuram", "mandapet", "samalkot", "pithapuram", "tenali", "macherla", "repalle", "vinukonda", "guntur", "chilakaluripet", "piduguralla", "sattenapalle", "narasaraopet", "ponnur", "mangalagiri", "bapatla", "tadepalli", "jaggaiahpet", "nandigama", "machilipatnam", "tiruvuru", "gudivada", "vuyyuru", "pedana", "vijayawada", "nuzividu", "gudurknl", "dhone", "allagadda", "adoni", "nandyal", "yemmiganur", "atmakurknl", "nandikotkur", "gudurnlr", "venkatagiri", "atmakurnlr", "nellore", "naidupet", "sullurpeta", "kavali", "cheemakurthy", "kanigiri", "giddalur", "addanki", "kandukur", "ongole", "markapur", "chirala", "srikakulam", "palasakasibugga", "rajam", "amudalavalasa", "ichapuram", "palakonda", "narsipatnam", "gvmcakp", "yelamanchili", "visakhapatnam", "gvmcbpm", "parvathipuram", "saluru", "vijayanagaram", "nellimarla", "bobbili", "tadepalligudem", "eluru", "narasapur", "kovvur", "tanuku", "palakol", "nidadavole", "jangareddygudem", "bhimavaram"]

        if (!token || !user || ($.inArray(type, uldArray) == -1)) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

       

        scope.ulb = type;

        scope.GetStatus = function () {

            scope.dttable = false;
            if (!scope.appno) {
                swal('info', "Please Enter Application Number.", 'info');
                return false;
            }

            else {
                scope.preloader = true;
                scope.dttable = false;

                var req = { ulb: scope.ulb, application_number: scope.appno };
                s_services.POSTENCRYPTAPI("GetMUIDAppStatus", req, token, function (value) {
                    console.log(value);
                    if (value.data.Status == 100) {
                        console.log(value.data.Details);
                        var data = value.data.Details;
                        if (data.remarks == "Transaction is not found") { swal('info', "No Data Found", 'info'); }
                        else {
                            scope.txn_id = data.txn_id;
                            scope.application_number = data.application_number;
                            scope.service_name = data.service_name;
                            scope.district = data.district;
                            scope.ulb = data.ulb;
                            scope.view_link = data.view_link;
                            scope.application_status = data.application_status;
                            scope.dttable = true;
                        }
                    }
                    else if (value.data.Status == "428") {
                        sessionStorage.clear();
                        swal("info", "Session Expired !!!", "info");
                        state.go("Login");
                        return;
                    }
                    else { swal('info', value.data.Reason, 'info'); }

                    scope.preloader = false;

                });
            }
        }

        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return (urlparam[1] ? urlparam[1].toLowerCase() : "");
                }
            }
        }
    }
})();