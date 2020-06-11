(function () {
	var app = angular.module("GSWS");

	app.controller("MeesevaController", ["$scope", "$state", "$log", "Meeseva_Services", Meeseva_CTRL]);

	function Meeseva_CTRL(scope, state, log, Meeseva_Services) {
		scope.preloader = false;

		scope.divmeeseva = true;




		scope.submit = function (val) {

			const url = "http://uat.meeseva.gov.in/GSVWIMeeseva/UserInterface/DC/VSWSRedirection.aspx";
			var req = { SERVICEID: val }
			Meeseva_Services.DemoAPI("MeesevaEncrypt", req, function (value) {

				var res = value.data;
				if (res.STATUS == "100") {
					scope.divmeeseva = true;

					scope.mtoken = res.TOKEN;
					//alert(scope.mtoken);
					scope.mlandingid = res.LANDINGID;
					scope.mscaid = res.SCAID;
					scope.mchannelid = res.CHANNELID;
					scope.moperatorid = res.OPERATORID;
					scope.moperator_uniqueno = res.OPERATOR_UNIQUENO;
					scope.mserviceid = res.SERVICEID;
					scope.mencdata = res.ENCDATA;

					$("#DYNAMICDATA").append($('<form id="member_signup"  action="http://uat.meeseva.gov.in/GSVWIMeeseva/UserInterface/DC/VSWSRedirection.aspx" name="member_signup" method="POST" >').append(

						$('<input />', { name: 'TOKEN', value: res.TOKEN, type: 'text' }),
						$('<input />', { name: 'LANDINGID', value: res.LANDINGID, type: 'text' }),
						$('<input />', { name: 'SCAID', value: res.SCAID, type: 'text' }),
						$('<input />', { name: 'CHANNELID', value: res.CHANNELID, type: 'text' }),
						$('<input />', { name: 'OPERATORID', value: res.OPERATORID, type: 'text' }),
						$('<input />', { name: 'OPERATOR_UNIQUENO', value: res.OPERATOR_UNIQUENO, type: 'text' }),
						$('<input />', { name: 'SERVICEID', value: res.SERVICEID, type: 'text' }),
						$('<input />', { name: 'ENCDATA', value: res.ENCDATA, type: 'text' }),
						$('<br />'),
						$('<input />', { id: 'savebutton', type: 'submit', value: 'Save' })), '</form>');

					document.forms['member_signup'].submit();

					// var newForm=jQuery('<form>',{ 'action':'http://uat.meeseva.gov.in/GSVWIMeeseva/UserInterface/DC/VSWSRedirection.aspx'}).
					// append(jQuery('<input>', { 'name': 'TOKEN', 'value': res.TOKEN, 'type': 'hidden' },
					// '<input>', { 'name': 'LANDINGID', 'value': res.LANDINGID, 'type': 'hidden' },
					// '<input>', { 'name': 'SCAID', 'value': res.SCAID, 'type': 'hidden' },
					// '<input>', { 'name': 'CHANNELID', 'value': res.CHANNELID, 'type': 'hidden' },
					// '<input>', { 'name': 'OPERATORID', 'value': res.OPERATORID, 'type': 'hidden' },
					// '<input>', { 'name': 'OPERATOR_UNIQUENO', 'value': res.OPERATOR_UNIQUENO, 'type': 'hidden' },
					// '<input>', { 'name': 'SERVICEID', 'value': res.SERVICEID, 'type': 'hidden' }));

					// newForm.submit();


				}
				else {
					scope.divmeeseva = false;
					swal('info', res.Message, 'info');
					return;
				}
			});

		}

	}
})();

