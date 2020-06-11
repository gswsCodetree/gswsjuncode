(function () {
	var app = angular.module("GSWS", ["Network"]);

	app.controller("LoginMainControllernew", ["$scope", "$interval", LoginMainControllernew]);

	function LoginMainControllernew(scope, int) {
		
		scope.navaratnaluImage = 0;
		scope.isPaused = false;
		scope.virusClick = function (id) {
			scope.navaratnaluImage = id;
		}
		int(function () {
			if (!scope.isPaused) {
				if (scope.navaratnaluImage == 9) {
					scope.navaratnaluImage = 0;
					return;
				}
				scope.navaratnaluImage++;
				return;
			}
		}, 5000);
		$('.navaratnalu-area').hover(function () {
			scope.isPaused = true;
		}, function () {
			scope.isPaused = false;
		});

		
	
		
		}
	
		
})();