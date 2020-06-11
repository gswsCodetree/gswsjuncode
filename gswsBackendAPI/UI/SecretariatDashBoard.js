(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("SecretariatDashBoardController", ["$scope", "$state", "$log", '$interval', "Login_Services", 'DTOptionsBuilder', GswsReportController]);

    function GswsReportController(scope, state, log, interval, Login_Services, DTOptionsBuilder) {
        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");
        var userloginrole = sessionStorage.getItem("desinagtion");
        if (!token || !user || (userloginrole != "110" && userloginrole != "206")) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }
        scope.SecretariatDD = [{ "SECRETARIATE_CODE": sessionStorage.getItem("secccode"), "SECRETARIATEL_NAME": sessionStorage.getItem("secname") }];
        scope.preloader = false;
        scope.type = "7";
        scope.ruflag = "";
        scope.seldistrict = sessionStorage.getItem("distcode");
        scope.selmandal = sessionStorage.getItem("mcode");
        scope.selsecratariat = sessionStorage.getItem("secccode");
        scope.selservicetype = "0";
        $("#pills-dept-tab").addClass("active");
        scope.fromdate = new Date();
        scope.todate = new Date();
        var newdate = new Date();
        newdate.setDate(newdate.getDate() - 10);
        scope.fromdate = newdate;

        scope.sendfromdate = moment().format('DD-MMM-YY');
        scope.sendfromdate = moment(scope.sendfromdate).add('-10', 'days').format('DD-MMM-YY')
        scope.sendtodate = moment().format('DD-MMM-YY');
        

        GetData();

        scope.dtInstance = {};

        scope.dtOptions = DTOptionsBuilder.newOptions().withOption('lengthMenu', [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]])
        
        //Load Static Department Dropdown
        scope.deptsList = [
            {
                "PS_SD_CODE": "11",
                "SD_NAME": "Agriculture and Marketing"
            },
            {
                "PS_SD_CODE": "12",
                "SD_NAME": "Animal Husbandry, Dairy Development and Fisheries"
            },
            {
                "PS_SD_CODE": "13",
                "SD_NAME": "Backward Classes Welfare"
            },
            {
                "PS_SD_CODE": "19",
                "SD_NAME": "Chief Electoral Officer"
            },
            {
                "PS_SD_CODE": "18",
                "SD_NAME": "Consumer Affairs, Food and Civil Supplies"
            },
            {
                "PS_SD_CODE": "32",
                "SD_NAME": "Disaster Management"
            },
            {
                "PS_SD_CODE": "14",
                "SD_NAME": "Environment, Forest, Science and Technology"
            },
            {
                "PS_SD_CODE": "16",
                "SD_NAME": "Energy"
            },
            {
                "PS_SD_CODE": "15",
                "SD_NAME": "Human Resources (Higher Education)"
            },
            {
                "PS_SD_CODE": "17",
                "SD_NAME": "Human Resources (School Education)"
            },
            {
                "PS_SD_CODE": "20",
                "SD_NAME": "Health, Medical & Family Welfare"
            },
            {
                "PS_SD_CODE": "21",
                "SD_NAME": "Home"
            },
            {
                "PS_SD_CODE": "22",
                "SD_NAME": "Housing"
            },
            {
                "PS_SD_CODE": "24",
                "SD_NAME": "Industries, Infrastructure, Investment and Commerce"
            },
            {
                "PS_SD_CODE": "25",
                "SD_NAME": "Information Technology, Electronics and Communications"
            },
            {
                "PS_SD_CODE": "40",
                "SD_NAME": "Law"
            },
            {
                "PS_SD_CODE": "26",
                "SD_NAME": "Labour, Employment, Training and Factories"
            },
            {
                "PS_SD_CODE": "27",
                "SD_NAME": "Municipal Administration and Urban Development"
            },
            {
                "PS_SD_CODE": "28",
                "SD_NAME": "Minorities Welfare"
            },
            {
                "PS_SD_CODE": "29",
                "SD_NAME": "Public Enterprises"
            },
            {
                "PS_SD_CODE": "30",
                "SD_NAME": "Planning"
            },
            {
                "PS_SD_CODE": "31",
                "SD_NAME": "Panchayat Raj and Rural Development"
            },
            {
                "PS_SD_CODE": "33",
                "SD_NAME": "Revenue"
            },
            {
                "PS_SD_CODE": "34",
                "SD_NAME": "Real Time Governance"
            },
            {
                "PS_SD_CODE": "35",
                "SD_NAME": "Skill Development, Entrepreneurship and Innovation"
            },
            {
                "PS_SD_CODE": "36",
                "SD_NAME": "Social & Tribal Welfare"
            },
            {
                "PS_SD_CODE": "37",
                "SD_NAME": "Transport, Roads and Buildings"
            },
            {
                "PS_SD_CODE": "38",
                "SD_NAME": "Women, Children, Disabled and Senior Citizens"
            },
            {
                "PS_SD_CODE": "23",
                "SD_NAME": "Water Resources"
            },
            {
                "PS_SD_CODE": "39",
                "SD_NAME": "Youth Advancement, Tourism and Culture"
            }
        ];

        //Load Static District Dropdown
        scope.districtsList = [
            {
                "LGD_DIST_CODE": "502",
                "DISTRICT_NAME": "ANANTHAPUR"
            },
            {
                "LGD_DIST_CODE": "503",
                "DISTRICT_NAME": "CHITTOOR"
            },
            {
                "LGD_DIST_CODE": "505",
                "DISTRICT_NAME": "EAST GODAVARI"
            },
            {
                "LGD_DIST_CODE": "506",
                "DISTRICT_NAME": "GUNTUR"
            },
            {
                "LGD_DIST_CODE": "504",
                "DISTRICT_NAME": "KADAPA"
            },
            {
                "LGD_DIST_CODE": "510",
                "DISTRICT_NAME": "KRISHNA"
            },
            {
                "LGD_DIST_CODE": "511",
                "DISTRICT_NAME": "KURNOOL"
            },
            {
                "LGD_DIST_CODE": "515",
                "DISTRICT_NAME": "POTTI SREERAMULU NELLORE"
            },
            {
                "LGD_DIST_CODE": "517",
                "DISTRICT_NAME": "PRAKASAM"
            },
            {
                "LGD_DIST_CODE": "519",
                "DISTRICT_NAME": "SRIKAKULAM"
            },
            {
                "LGD_DIST_CODE": "520",
                "DISTRICT_NAME": "VISAKHAPATNAM"
            },
            {
                "LGD_DIST_CODE": "521",
                "DISTRICT_NAME": "VIZIANAGARAM"
            },
            {
                "LGD_DIST_CODE": "523",
                "DISTRICT_NAME": "WEST GODAVARI"
            }
        ];

        scope.GetData = function () {
            GetData();
        }

        function GetData() {
            scope.type = "7";
            var req = {
                TYPE: scope.type,
                FROMDATE: scope.sendfromdate,
                TODATE: scope.sendtodate,
                DISTRICT: scope.seldistrict,
                MANDAL: scope.selmandal,
                SECRETARIAT: scope.selsecratariat,
                DEPARTMENT: scope.selecteddepartment,
                FLAG: scope.selruflag,
                SERVICE: scope.selectedservice,
                SERVICE_TYPE: scope.selservicetype,
            }
            scope.preloader = true;
            Login_Services.POSTENCRYPTAPI("GetSecretariatReport", req, token, function (data) {

                var res = data.data;
                scope.dtInstance = null;

                scope.SecretariatWiseReport = [];
                scope.SecviceWiseReport = [];

                scope.TOTAL_STATUS_CLICKS = 0;
                scope.TOTAL_REG_CLICKS = 0;
                scope.TOTAL_NO_OF_SER_REQ = 0;
                scope.TOTAL_OPEN_WITHIN_SLA = 0;
                scope.TOTAL_OPEN_WITHIN_SLA_PER = 0;
                scope.TOTAL_OPEN_BEYOND_SLA = 0;
                scope.TOTAL_OPEN_BEYOND_SLA_PER = 0;
                scope.TOTAL_CLOSE_WITHIN_SLA = 0;
                scope.TOTAL_CLOSE_WITHIN_SLA_PER = 0;
                scope.TOTAL_CLOSE_BEYOND_SLA = 0;
                scope.TOTAL_CLOSE_BEYOND_SLA_PER = 0;
                scope.SER_REQ_OPEN_PERCENTAGE = 0;
                scope.SER_REQ_CLOSE_PERCENTAGE = 0;
                scope.TOTAL_GRID_OPEN_WITHIN_SLA_PER = 0;
                scope.TOTAL_GRID_OPEN_BEYOND_SLA_PER = 0;
                scope.TOTAL_GRID_CLOSE_WITHIN_SLA_PER = 0;
                scope.TOTAL_GRID_CLOSE_BEYOND_SLA_PER = 0;

                scope.TOTAL_OPEN = 0;
                scope.TOTAL_CLOSE = 0;
                scope.SER_REQ_PERCENTAGE = 0
                scope.TOTAL_OPEN_PER = 0;
                scope.TOTAL_CLOSE_PER = 0;
                scope.piechartarray = [];

                if (res.Status == '100') {
                    var k = 0;
                    for (var i = 0; i < res.DataList.length; i++) {
                        k++;

                        if (scope.type == "7")
                            scope.SecretariatWiseReport.push(res.DataList[i]);
                        

                        scope.TOTAL_NO_OF_SER_REQ += parseInt(res.DataList[i]["SER_REQUESTED"]);
                        scope.TOTAL_OPEN_WITHIN_SLA += parseInt(res.DataList[i]["OPEN_WITHIN_SLA"]);
                        scope.TOTAL_OPEN_BEYOND_SLA += parseInt(res.DataList[i]["OPEN_BEYOND_SLA"]);
                        scope.TOTAL_CLOSE_WITHIN_SLA += parseInt(res.DataList[i]["CLOSED_WITHIN_SLA"]);
                        scope.TOTAL_CLOSE_BEYOND_SLA += parseInt(res.DataList[i]["CLOSED_BEYOND_SLA"]);

                        if (res.DataList.length == k) {
                            scope.TOTAL_OPEN = scope.TOTAL_OPEN_WITHIN_SLA + scope.TOTAL_OPEN_BEYOND_SLA;
                            scope.TOTAL_CLOSE = scope.TOTAL_CLOSE_WITHIN_SLA + scope.TOTAL_CLOSE_BEYOND_SLA;


                            scope.TOTAL_OPEN_WITHIN_SLA_PER = scope.TOTAL_OPEN_WITHIN_SLA / scope.TOTAL_OPEN * 100;
                            scope.TOTAL_OPEN_BEYOND_SLA_PER = scope.TOTAL_OPEN_BEYOND_SLA / scope.TOTAL_OPEN * 100;
                            scope.TOTAL_CLOSE_WITHIN_SLA_PER = scope.TOTAL_CLOSE_WITHIN_SLA / scope.TOTAL_CLOSE * 100;
                            scope.TOTAL_CLOSE_BEYOND_SLA_PER = scope.TOTAL_CLOSE_BEYOND_SLA / scope.TOTAL_CLOSE * 100;


                            scope.TOTAL_GRID_OPEN_WITHIN_SLA_PER = scope.TOTAL_OPEN_WITHIN_SLA / scope.TOTAL_NO_OF_SER_REQ * 100;
                            scope.TOTAL_GRID_OPEN_BEYOND_SLA_PER = scope.TOTAL_OPEN_BEYOND_SLA / scope.TOTAL_NO_OF_SER_REQ * 100;
                            scope.TOTAL_GRID_CLOSE_WITHIN_SLA_PER = scope.TOTAL_CLOSE_WITHIN_SLA / scope.TOTAL_NO_OF_SER_REQ * 100;
                            scope.TOTAL_GRID_CLOSE_BEYOND_SLA_PER = scope.TOTAL_CLOSE_BEYOND_SLA / scope.TOTAL_NO_OF_SER_REQ * 100;

                            scope.SER_REQ_OPEN_PERCENTAGE = scope.TOTAL_OPEN / scope.TOTAL_NO_OF_SER_REQ * 100;
                            scope.SER_REQ_CLOSE_PERCENTAGE = scope.TOTAL_CLOSE / scope.TOTAL_NO_OF_SER_REQ * 100;
                        }

                        
                       
                    }

                    percentagefill();
                    piechart();

                    
                    scope.preloader = false;
                }
                else {
                    scope.preloader = false;
                    swal("", res.Reason, "success");
                }
                scope.preloader = false;
            });
        }

        function percentagefill() {
            $("#per1").each(function () {
                var value = scope.SER_REQ_OPEN_PERCENTAGE;
                var left = $(this).find('.progress-left .progress-bar');
                var right = $(this).find('.progress-right .progress-bar');

                if (value > 0) {
                    if (value <= 50) {
                        left.css('transform', 'rotate(' + 0 + 'deg)')
                        right.css('transform', 'rotate(' + percentageToDegrees(value) + 'deg)')
                    } else {
                        right.css('transform', 'rotate(180deg)')
                        left.css('transform', 'rotate(' + percentageToDegrees(value - 50) + 'deg)')
                    }
                }
                else {
                    left.css('transform', 'rotate(' + 0 + 'deg)')
                    right.css('transform', 'rotate(' + 0 + 'deg)')
                }
            })

            $("#per2").each(function () {
                var value = scope.SER_REQ_CLOSE_PERCENTAGE;
                var left = $(this).find('.progress-left .progress-bar');
                var right = $(this).find('.progress-right .progress-bar');

                if (value > 0) {
                    if (value <= 50) {
                        left.css('transform', 'rotate(' + 0 + 'deg)')
                        right.css('transform', 'rotate(' + percentageToDegrees(value) + 'deg)')
                    } else {
                        right.css('transform', 'rotate(180deg)')
                        left.css('transform', 'rotate(' + percentageToDegrees(value - 50) + 'deg)')
                    }
                }
                else {
                    left.css('transform', 'rotate(' + 0 + 'deg)')
                    right.css('transform', 'rotate(' + 0 + 'deg)')
                }
            })

            $("#per3").each(function () {
                var value = scope.TOTAL_OPEN_WITHIN_SLA_PER;
                var left = $(this).find('.progress-left .progress-bar');
                var right = $(this).find('.progress-right .progress-bar');

                if (value > 0) {
                    if (value <= 50) {
                        left.css('transform', 'rotate(' + 0 + 'deg)')
                        right.css('transform', 'rotate(' + percentageToDegrees(value) + 'deg)')
                    } else {
                        right.css('transform', 'rotate(180deg)')
                        left.css('transform', 'rotate(' + percentageToDegrees(value - 50) + 'deg)')
                    }
                }
                else {
                    left.css('transform', 'rotate(' + 0 + 'deg)')
                    right.css('transform', 'rotate(' + 0 + 'deg)')
                }
            })

            $("#per4").each(function () {
                var value = scope.TOTAL_OPEN_BEYOND_SLA_PER;
                var left = $(this).find('.progress-left .progress-bar');
                var right = $(this).find('.progress-right .progress-bar');

                if (value > 0) {
                    if (value <= 50) {
                        left.css('transform', 'rotate(' + 0 + 'deg)')
                        right.css('transform', 'rotate(' + percentageToDegrees(value) + 'deg)')
                    } else {
                        right.css('transform', 'rotate(180deg)')
                        left.css('transform', 'rotate(' + percentageToDegrees(value - 50) + 'deg)')
                    }
                }
                else {
                    left.css('transform', 'rotate(' + 0 + 'deg)')
                    right.css('transform', 'rotate(' + 0 + 'deg)')
                }
            })

            $("#per5").each(function () {
                var value = scope.TOTAL_CLOSE_WITHIN_SLA_PER;
                var left = $(this).find('.progress-left .progress-bar');
                var right = $(this).find('.progress-right .progress-bar');

                if (value > 0) {
                    if (value <= 50) {
                        left.css('transform', 'rotate(' + 0 + 'deg)')
                        right.css('transform', 'rotate(' + percentageToDegrees(value) + 'deg)')
                    } else {
                        right.css('transform', 'rotate(180deg)')
                        left.css('transform', 'rotate(' + percentageToDegrees(value - 50) + 'deg)')
                    }
                }
                else {
                    left.css('transform', 'rotate(' + 0 + 'deg)')
                    right.css('transform', 'rotate(' + 0 + 'deg)')
                }
            })

            $("#per6").each(function () {
                var value = scope.TOTAL_CLOSE_BEYOND_SLA_PER;
                var left = $(this).find('.progress-left .progress-bar');
                var right = $(this).find('.progress-right .progress-bar');

                if (value > 0) {
                    if (value <= 50) {
                        left.css('transform', 'rotate(' + 0 + 'deg)')
                        right.css('transform', 'rotate(' + percentageToDegrees(value) + 'deg)')
                    } else {
                        right.css('transform', 'rotate(180deg)')
                        left.css('transform', 'rotate(' + percentageToDegrees(value - 50) + 'deg)')
                    }
                }
                else {
                    left.css('transform', 'rotate(' + 0 + 'deg)')
                    right.css('transform', 'rotate(' + 0 + 'deg)')
                }
            })

            function percentageToDegrees(percentage) {
                return percentage / 100 * 360
            }

        }

        //Change in Dates
        scope.changedate = function (d) {
            if (d == 1) {
                scope.sendfromdate = moment(scope.fromdate).format('DD-MMM-YY');
            }
            else if (d == 2) {
                scope.sendtodate = moment(scope.todate).format('DD-MMM-YY');
            }
        }

        //Department change
        scope.deptChange = function () {
            LoadServices();
        }

        scope.BackBtnClick = function () {
            scope.type = "7";
            GetData();
        }

        scope.CountClick = function (obj) {
            scope.type = "8";
            var req = {
                TYPE: scope.type,
                FROMDATE: scope.sendfromdate,
                TODATE: scope.sendtodate,
                DISTRICT: scope.seldistrict,
                MANDAL: scope.selmandal,
                SECRETARIAT: scope.selsecratariat,
                DEPARTMENT: scope.selecteddepartment,
                FLAG: scope.selruflag,
                SERVICE: obj.SERVICECODE,
                SERVICE_TYPE: scope.selservicetype,
            }
            scope.preloader = true;
            Login_Services.POSTENCRYPTAPI("GetSecretariatReport", req, token, function (data) {
                var res = data.data;
                scope.SecretariatWiseReport = [];
                scope.ServiceWiseReport = [];

                if (res.Status == '100') {
                    scope.ServiceWiseReport = (res.DataList);
                    scope.preloader = false;
                }
                else {
                    scope.preloader = false;
                    swal("", res.Reason, "success");
                }
                scope.preloader = false;
            });
        }


        $("#changedep").change(function () {
            scope.selecteddepartment = $("#changedep").val();
            scope.deptChange();
        });

        $("#changeser").change(function () {
            scope.selectedservice = $("#changeser").val();
        });

        function LoadServices() {
            var req = {
                TYPE: "99",
                FROMDATE: scope.sendfromdate,
                TODATE: scope.sendtodate,
                DISTRICT: scope.seldistrict,
                MANDAL: scope.selmandal,
                SECRETARIAT: scope.selsecratariat,
                DEPARTMENT: scope.selecteddepartment,
                FLAG: scope.selruflag,
                SERVICE: scope.selectedservice
            }
            scope.preloader = true;
            Login_Services.POSTENCRYPTAPI("GetSecretariatReport", req, token, function (data) {
                var res = data.data;
                if (res.Status == "100") {
                    scope.preloader = false;
                    scope.servicesList = res.DataList;
                }
                else {
                    scope.preloader = false;
                }
                scope.preloader = false;
            });
        }

        function piechart() {
            var ctx = document.getElementById("myChart").getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: ["Open within SLA", "Open Beyond SLA", "Closed within SLA", "Closed Beyond SLA"],
                    datasets: [{
                        backgroundColor: [
                            "#2ecc71",
                            "#3498db",
                            "#f1c40f",
                            "#e74c3c"
                        ],
                        data: [scope.TOTAL_OPEN_WITHIN_SLA, scope.TOTAL_OPEN_BEYOND_SLA, scope.TOTAL_CLOSE_WITHIN_SLA, scope.TOTAL_CLOSE_BEYOND_SLA]
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    legend: {
                        display: true,
                        position: 'bottom',
                        fullWidth: true,
                        labels: {
                            boxWidth: 20,
                            fontSize: 10,
                            fontColor: '#000'
                        }
                    },
                    title: {
                        display: true,
                        text: 'Custom Chart Title'
                    }
                }
            });
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Excel Download

        scope.ExcelDownload = function (e) {
             if (e == "SECRETARIAT") {
                $("#SECRETARIAT").table2excel({
                    name: "Worksheet Name",
                    exclude: ".image",
                    filename: scope.table
                });
            }
            else if (e == "DEPARTMENT") {
                $("#DEPARTMENT").table2excel({
                    name: "Worksheet Name",
                    exclude: ".image",
                    filename: scope.table
                });
            }
            else if (e == "SERVICE") {
                $("#SERVICE").table2excel({
                    name: "Worksheet Name",
                    exclude: ".image",
                    filename: scope.table
                });
            }
            else if (e == "SERVICE6") {
                $("#SERVICE6").table2excel({
                    name: "Worksheet Name",
                    exclude: ".image",
                    filename: scope.table
                });
            }
        }

        //sorting
        scope.reverse = false;

        // called on header click
        scope.sortColumn = function (col) {
            scope.column = col;
            if (scope.reverse) {
                scope.reverse = false;
                scope.reverseclass = 'arrow-up';
            } else {
                scope.reverse = true;
                scope.reverseclass = 'arrow-down';
            }
        };

        // remove and change class
        scope.sortClass = function (col) {
            if (scope.column == col) {
                if (!scope.reverse) {
                    return 'sortable asc';
                } else {
                    return 'sortable desc';
                }
            } else {
                return 'sortable asc desc';
            }
        }

        

        scope.sortClass('');

        scope.setOrderProperty = function (propertyName) {
            if (scope.orderProperty === propertyName) {
                scope.orderProperty = '-' + propertyName;
            } else if (scope.orderProperty === '-' + propertyName) {
                scope.orderProperty = propertyName;
            } else {
                scope.orderProperty = propertyName;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Service6 PDF
        scope.Service6Export = function (elem) {
            var tab = document.getElementById(elem);
            var win = window.open('', '', 'height=700,width=700');
            win.document.write(tab.outerHTML);
            win.document.close();
            win.print();
        }
       
    }
})();