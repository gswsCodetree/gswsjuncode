(function () {
    var status = false;
    var app = angular.module("GSWS");

    app.controller("FMRegistration", ["$scope", "$state", "$log", "AgriCulture_Services", Agri_CTRL]);

    function Agri_CTRL(scope, state, log, fmr_services) {
        scope.pagename = "Former Mechanization";
        scope.preloader = false;
        var token = sessionStorage.getItem("Token");
		var user = sessionStorage.getItem("user");

		if (!token || !user) {
			state.go("Login");
			return;
		}
        scope.FMSubDistrictsDD = [];
        scope.FMBlocksDD = [];
        scope.FMVillagesDD = [];

        scope.FMDistrictsDD = FMDistricts;

        scope.ChangeDistrict = function () {
            scope.FMSubDistrictsDD = [];
            scope.FMBlocksDD = [];
            scope.FMVillagesDD = [];

            if (scope.selDistrict) {
                scope.FMSubDistrictsDD = $(FMSubDistricts).filter(function (i, n) { return n.DistrictCode === scope.selDistrict });
                scope.FMBlocksDD = $(FMBlocks).filter(function (i, n) { return n.DistrictCode === scope.selDistrict });
            }
        }

        scope.ChangeSubDistrict = function () {
            scope.FMVillagesDD = [];
            if (scope.selSubDistrict)
                scope.FMVillagesDD = $(FMVillages).filter(function (i, n) { return n.SubdistCode === scope.selSubDistrict });
        }

        scope.Register = function () {
            if (Validation()) {
                scope.preloader = true;
                var req = {
                    DistrictCode: scope.selDistrict,
                    SubDistrictCode: scope.selSubDistrict,
                    BlockCode: scope.selBlock,
                    VillageCode: scope.selVillage,
                    FarmerName: scope.FMName,
                    FatherHusbandName: scope.FMFName,
                    DOB: moment(scope.FMDOB).format('DD/MM/YYYY'),
                    Gender: scope.selGender,
                    CasteCategory: scope.selCategory,
                    FarmerType: scope.selFType,
                    AadharNo: scope.FMAadhaar,
                    MobileNo: scope.FMMobile,
                    Phone: scope.FMPhone,
                    EmailId: scope.FMEmail,
                    Address: scope.FMAddress,
                    PinCode: scope.FMPin,
                    PAN: scope.FMPAN
                };
                fmr_services.POSTENCRYPTAPIAGRI("FMAppRegister", req, token, function (value) {
                    scope.StudentDetails = [];
                    if (value.data.Status == 100) {
                        console.log(value.data.Details);
                        swal({
                            title: "Success",
                            text: "Farmer Registred Successfully." + " \n USER ID : " + value.data.Details.UserID + "\n PASSWORD : " + value.data.Details.Password + "\n Central Unique ID : " + value.data.Details.CentralUnique_BenID,
                            icon: "success",
                            buttons: true,
                            dangerMode: true,
                        }).then((willDelete) => {
                            if (willDelete) {
                                window.location.reload();

                            } else {
                                window.location.reload();
                            }
                            });

                        //swal('info', "Farmer Registred Successfully." + " \n USER ID : " + value.data.Details.UserID + "\n PASSWORD : " + value.data.Details.Password + "\n Central Unique ID : " + value.data.Details.CentralUnique_BenID, 'info');

                        //setTimeout(function () { window.location.reload(); }, 5000);
					}
					else if (res.Status == "428") {
						sessionStorage.clear();
						state.go("Login");
						return;
					}
                    else { swal('info', value.data.Reason, 'info'); }

                    scope.preloader = false;

                });
            }
        }

        function Validation() {
            var emailRegex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

            if (!scope.selDistrict) {
                swal("Info", "Please Select District", "info");
                return false;
            }
            else if (!scope.selSubDistrict) {
                swal("Info", "Please Select Sub District", "info");
                return false;
            }
            else if (!scope.selBlock) {
                swal("Info", "Please Select Block", "info");
                return false;
            }
            else if (!scope.selVillage) {
                swal("Info", "Please Select Village", "info");
                return false;
            }
            else if (!scope.FMName) {
                swal("Info", "Please Enter Name", "info");
                return false;
            }
            else if (!scope.FMFName) {
                swal("Info", "Please Enter Father/Husband", "info");
                return false;
            }
            else if (!scope.FMDOB) {
                swal("Info", "Please Enter Date of Birth", "info");
                return false;
            }
            else if (!scope.selGender) {
                swal("Info", "Please Select Gender", "info");
                return false;
            }
            else if (!scope.selCategory) {
                swal("Info", "Please Select Category", "info");
                return false;
            }
            else if (!scope.selFType) {
                swal("Info", "Please Select Farmer Type", "info");
                return false;
            }
            else if (!scope.FMAadhaar) {
                swal("Info", "Please Enter Aadhaar", "info");
                return false;
            }
            else if (!scope.FMMobile) {
                swal("Info", "Please Enter Mobile", "info");
                return false;
            }
            else if (scope.FMMobile.length < 10) {
                swal("Info", "Mobile Should be 10 digits ", "info");
                return false;
            }
            else if (scope.FMEmail && !emailRegex.test(scope.FMEmail)) {
                swal("Info", "Invalid Email", "info");
                return false;
            }
            else if (!scope.FMAddress) {
                swal("Info", "Please Enter Address", "info");
                return false;
            }
            else if (!scope.FMPin) {
                swal("Info", "Please Enter PIN", "info");
                return false;
            }

            return true;
        }

    }
})();