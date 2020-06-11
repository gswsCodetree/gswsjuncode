var List = [];

(function () {
    var app = angular.module("GSWS");

    app.controller("ExciseComplaintController", ["$scope", "$state", "$log", "REVENUE_Services", Ration_CTRL]);

    function Ration_CTRL(scope, state, log, REVENUE_Services) {

        var token = sessionStorage.getItem("Token");
        var user = sessionStorage.getItem("user");

        if (!token || !user) {
            sessionStorage.clear();
            state.go("Login");
            return false;
        }

        LoadDistricts();


        scope.SaveInfo = function () {
            List = [];
            validationchecks();

        };

        function validationchecks() {

            //NM
            //if (!(scope.txtname)) {
            //	alert("Please enter applicant name");
            //	return;
            //}
            //M
            if (!(scope.txtmobnum)) {
                alert("Please enter applicant mobile number");
                return;
            }
            if (scope.txtmobnum.length != 10) {
                alert("Please enter 10 digit applicant mobile number");
                return;
            }
            //NM
            if ((scope.txtaadhaar)) {
                if (scope.txtaadhaar.length != 12) {
                    alert("Please enter 12 digit applicant aadhaar number");
                    return;
                }
            }

            //M
            if (!(scope.distdata)) {
                alert("Please select district");
                return;
            }
            //M
            if (!(scope.mandaldata)) {
                alert("Please select mandal");
                return;
            }
            //M
            if (!(scope.villagedata)) {
                alert("Please select village");
                return;
            }
            //M
            if (!(scope.txtcomplaintdetails)) {
                alert("Please enter Complaint details");
                return;
            }
            var tr_no = Math.floor(100000 + Math.random() * 900000);
            FinalCheck();
            var ResultInputObject = {
				TransactionID: tr_no, ApplicantName: scope.txtname, ApplicantMobileNumber: scope.txtmobnum, ApplicantAadhaar: scope.txtaadhaar, GSWS_ID: sessionStorage.getItem("TransID"),
                DistrictCode: scope.distdata, MandalCode: scope.mandaldata, VillageCode: scope.villagedata, ComplaintDetails: scope.txtcomplaintdetails, UploadDoc: List
            };
            SaveData(ResultInputObject);
        }


        function SaveData(objInput) {
            REVENUE_Services.DemoAPI("SaveExcisePCF", objInput, function (value) {
                var res = value.data;
                if (res.Status == "success") {
                    swal('success', 'Your Complaint has been registered. Complaint ID:' + res.Data, 'success');
                    resetdata();
                    return;
                }
                else {
					swal('error', res.Reason, 'error');
                    return;
                }
            });
        }

        scope.Reset = function () {
            location.reload(true);
        }

        function resetdata() {
            scope.txtname = "";
            scope.txtmobnum = "";
            scope.txtaadhaar = "";
            scope.distdata = "";
            scope.mandaldata = "";
            scope.Mandalslist = "";
            scope.villagedata = "";
            scope.Villageslist = "";
            scope.txtcomplaintdetails = "";
            List = [];
            LoadDistricts();
        }

        function LoadDistricts() {
            var obj = {}
            REVENUE_Services.POSTREVENCRYPTAPI("Excise_DistrictLoad", obj, token, function (value) {
                var res = value.data;
                if (res.Status == "Success") {
                    scope.Districtslist = res.Data.districtList;
                }
                else {
                    scope.Districtslist = "";
                    alert("No Districts Found");
                }
            });
        }
        scope.LoadMandals = function (dist) {
            if (!(dist)) {
                scope.Mandalslist = "";
                scope.Villageslist = "";
                alert('Select District');
            }
            var req = {
                distcode: dist
            };
            REVENUE_Services.POSTREVENCRYPTAPI("Excise_MandalsLoad", req, token, function (value) {
                var res = value.data;
                if (res.Status == "Success") {
                    scope.Mandalslist = res.Data.mandalList;
                }
                else {
                    scope.Districtslist = "";
                    alert("No Mandals Found");
                    return;
                }
            });
        }
        scope.LoadVillages = function (mandal) {
            if (!(mandal)) {
                scope.Villageslist = "";
                alert('Select Mandal');
            }
            var req = {
                mandalcode: mandal
            };
            REVENUE_Services.POSTREVENCRYPTAPI("Excise_VillagesLoad", req, token, function (value) {
                var res = value.data;
                if (res.Status == "Success") {
                    scope.Villageslist = res.Data.villagesList;
                }
                else {
                    scope.Districtslist = "";
                    alert("No Villages Found");
                    return;
                }
            });
        }
    }

})();






///////////////////////////////////////
function addAttachmentRowForPublicForm() {

    var objId = '#attachmentTable';

    var attachmentRowCounter = $(objId + " .attachmentStyle").length;

    $(objId + "")
        .append(
            '<tr class="attachmentStyle"><td colspan="2">'
            + '<input type="file"  id="fileAttachments'
            + attachmentRowCounter
            + '" name="fileAttachments['
            + attachmentRowCounter
            + ']" onchange="encodeImagetoBase64(this,' + attachmentRowCounter + ')" /><input type="hidden" id="hidden' + attachmentRowCounter + '">'
            + '&nbsp;&nbsp;&nbsp;'
            + '<a href="javascript:void(0);" style="text-decoration:none;" onclick="deleteAttachmentRowPublicForm();"><font color="gray">-Delete Attachment</font></a>'
            + '</td><td  colspan="4"><input id="attachmentComments[' + attachmentRowCounter + ']" name="attachmentComments[' + attachmentRowCounter + ']" type="text" path="attachmentComments[' + attachmentRowCounter + ']" value=""/></td>');
}

function deleteAttachmentRowPublicForm() {
    var objId = '#attachmentTable';
    $(objId + " tr").eq(-1).remove();
}
function validationComplaintForm() {

    var objId = '#attachmentTable';
    var formId = '#publicControlRoomBeanId';
    var attachmentRowCounter = $(objId + " .attachmentStyle").length;
    //alert("attachmentRowCounter"+attachmentRowCounter)

    var i = 0, flag = 0;
    for (i = 0; i < attachmentRowCounter; i++) {
        //	alert("i"+i);


        var file = $('#fileAttachments' + i + '\\.file').val();


        if (file.match(/\.([^\.]+)$/) != null) {

            var ext = file.match(/\.([^\.]+)$/)[1];
            switch (ext) {
                case 'jpg':
                    $('#fileAttachments' + i + '\\.file').css('border', '');
                    break;
                case 'pdf':

                    $('#fileAttachments' + i + '\\.file').css('border', '');
                    break;
                case 'jpeg':

                    $('#fileAttachments' + i + '\\.file').css('border', '');
                    break;
                case 'png':

                    $('#fileAttachments' + i + '\\.file').css('border', '');
                    break;
                default:
                    alert('Please choose attachment file type of PDF/JPG/JPEG/PNG only');
                    file = '';
                    $('#fileAttachments' + i + '\\.file').focus();
                    $('#fileAttachments' + i + '\\.file').css('border', 'solid 2px #FF0000');
                    return false;
            }


        }

    }
    return true;
}

function FinalCheck() {


    var objId = '#attachmentTable';
    var attachmentRowCounter = $(objId + " .attachmentStyle").length;
    //alert("attachmentRowCounter"+attachmentRowCounter)

    var i = 0, flag = 0;
    for (i = 0; i < attachmentRowCounter; i++) {
        var file = $('#fileAttachments' + i).val();

        if (file.match(/\.([^\.]+)$/) != null) {
            var items = {};
            //File Uploader
            var imagebase64;
            var filename = file.replace(/^.*\\/, "");
            var input_ = 'fileAttachments' + i;
            var text_COMMENT = document.getElementById('attachmentComments[' + i + ']').value; //$('#attachmentComments[' + i + ']').text();

            var img_bs64 = document.getElementById('hidden' + i).value;


            items.FileName = filename;
            items.Image = img_bs64;
            items.Comments = text_COMMENT;
            List.push(items);
        }
    }


}


function encodeImagetoBase64(element, h_id) {
    var file = element.files[0];
    var fileid = $('#fileAttachments' + h_id).val();
    if (fileid.match(/\.([^\.]+)$/) != null) {
        var ext = fileid.match(/\.([^\.]+)$/)[1];
        if (ext != "jpg" && ext != "pdf" && ext != "jpeg" && ext != "png") {
            alert('Please choose attachment file type of PDF/JPG/JPEG/PNG only');
            $('#fileAttachments' + h_id).val(''); file = '';
            return;
        } else {
            var reader = new FileReader();

            reader.onloadend = function () {
                //$(".link").attr("href", reader.result);
                $("#hidden" + h_id + "").val(reader.result);

            }
            reader.readAsDataURL(file);
        }
    }

}


$("btntest").click(function () {
    alert("This was clicked.");
});

