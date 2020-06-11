
$("#btnGetReport").click(function () {
	Get_Click();
	return false;
});

function Get_Click() {
	var txtcertid = document.getElementById("txtcertid").value;

	if (txtcertid == undefined || txtcertid == null || txtcertid == "") {
		alert('Please enter certificate number');
		return false;
	}
	else {
		CallWService('/API/SocialWelfare_Tribal/CertificatesDownload', JSON.stringify({ 'strIntegratedID': txtcertid, 'CertType': 'INTEGRATED' }), function (res) {
			if (res == undefined) {
				alert('Something went wrong.Please try again');
				return;
			}
			var resdata = res;
			if (resdata.Status == "Success") {
				dispdf.src = resdata.Data;
			}
		});
	}

	return false;
}



//Service Calling
function CallWService(u, req, onSuccess) {
	$.ajax({
		type: "POST",
		url: u,
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		data: req,
		success: function (data) {
			//onSuccess(jQuery.parseJSON(data.d));
			onSuccess(data);
		},
		error: function (xhr, status, error) {
			var err = eval("(" + xhr.responseText + ")");
			//$.jGrowl('There is a problem while processing your request.', { header: 'Error', life: 10000 });
		}
	});
}