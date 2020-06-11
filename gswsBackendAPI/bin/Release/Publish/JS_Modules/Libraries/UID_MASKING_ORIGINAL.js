/*
 Author  : Venkatesh Nelli
 Version : 3.0
 License : GNU General Public License v3.0
 Description: This Directive is used for masking aadhaar number and mobile number
 */
(function () {
    'use strict';
    var app = angular.module('input_masking', []);

    app.directive('uidInputModel', function () {


        function aadhaarInput_function($scope, $element, $attrs, $parse) {

            //creating a buffer variable
            $scope.val_name = "";

            //To get Entered uid-input-model name
            var input_model_name = $attrs.uidInputModel;

            // Get the data model
            var model = $parse(input_model_name);

            //reads the mask length from attribute
            var mask_length = $attrs.maskLength;

            // For long press only one time take key value and stops from continues interation
            var flag = 0;

            //Defined special keys array
            var specialKeys = new Array();

            //Push backspace as a special key
            specialKeys.push(8);

            //Triggers when key pressed
            $element.bind("keypress", function (e) {

                //checks until the input tag length is less than 12 characters
                if ($('input[uid-input-model=' + $attrs.uidInputModel + ']').val().length < 12) {

                    var keyCode = e.which ? e.which : e.keyCode;

                    //reads the entered key is number or not if number ret is true
                    var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);

                    if (ret) {

                        //adds the number to  scope variable as a buffer which is used to store the number temperorily
                        $scope.val_name += String.fromCharCode(keyCode);
                    }

                    //assign the final data to the data model
                    model.assign($scope, $scope.val_name);

                    return ret;
                }
            });

            //Triggers before key released
            $element.bind("keyup", function (e) {

                //When first time keypress flag initializes to zero
                flag = 0;

                /*
                //checks the input tag length using data model
                if ($('input[uid-input-model=' + $attrs.uidInputModel + ']').val().length == 12) {

                    //gets the data from the data model
                    var data = $scope.$eval($attrs.uidInputModel);

                    //validates the data model using verhoeff algorithm and if fails below condition executed to empty the input tag
                    if (!validateVerhoeff(data)) {

                        //empty the input tag
                        $('input[uid-input-model=' + $attrs.uidInputModel + ']').val("");

                        //empty the data model
                        model.assign($scope, "");

                        //shows alert to enter valid aadhaar number
                        alert("Entered aadhaar number is invalid");
                    }
                }
                */

                //function to fix android chrome bug which doesn't catch the entered input from key 
                var getKeyCode = function (str) {

                    //adds the key value to the scope varible which is further added to data model
                    $scope.val_name += str.substr(str.length - 1);

                    //if (str.length != str.length) {
                    //    $scope.val_name = $scope.val_name.substring(0, str.length);
                    //}

                };


                //for android chrome keycode fix only executes in chrome browser
                if (navigator.userAgent.match(/Android/i)) {

                    //reads the entered key value
                    var inputValue = this.value;
                    getKeyCode(inputValue);
                }


                //To get last entered character with complete string ex : 'XXXX5' 
                var a_val = $('input[uid-input-model=' + $attrs.uidInputModel + ']').val();


                //if mask length is null replaces last character with 'X'
                if (mask_length == undefined || mask_length == null || mask_length == "") {
                    a_val = this.value.replace(/[^\d\X]/g, '').replace(/\d/g, 'X');

                    //adds modified data to input tag
                    $('input[uid-input-model=' + $attrs.uidInputModel + ']').val(a_val);
                }
                else {
                    //if mask length is not null checks length replaces  with 'X' only until mentioned input length
                    //replaces with 'X' only until mentioned length
                    if (a_val.length <= mask_length) {
                        a_val = this.value.replace(/[^\d\X]/g, '').replace(/\d/g, 'X');
                    }

                    //adds modified data to input tag
                    $('input[uid-input-model=' + $attrs.uidInputModel + ']').val(a_val);

                }


                //If length of input tag text is not matched with the data model trims the data model upto the length of input tag
                if (a_val.length != $scope.val_name.length) {
                    $scope.val_name = $scope.val_name.substring(0, a_val.length);
                }

                //assign the final data to the data model
                model.assign($scope, $scope.val_name);


            });

            //Triggers after key pressed
            $element.keydown(function (e) {

                //When first time keypress flag incremented and prevent second time entering when log press
                flag++;

                //reads key value
                var key = e.keyCode || e.charCode;

                //check wether key is backspace
                if (key != 8) {

                    //Prevents long press repeated iteration
                    if (flag > 1) {
                        //prevents key entering
                        e.preventDefault();
                    }
                }
                else {
                    if ($('input[uid-input-model=' + $attrs.uidInputModel + ']').val().length > e.target.selectionStart) {
                        e.preventDefault();
                    }
                }
                //check wether key is dot
                if (key == 46) {
                    e.preventDefault();
                    e.stopPropagation();
                }

                //check wether key is left arrow
                if (key == 37) {
                    e.preventDefault();
                }

                //check wether key is right arrow
                if (key == 39) {
                    e.preventDefault();
                }

            });

            //it disables drop function
            $element.bind("drop", function (e) {
                return false;
            });

            //it disables pasting the data function
            $element.bind("paste", function (e) {
                return false;
            });

            //watches for any change of data model from code
            $scope.$watch(model, function (new_data, old_data) {

                if (new_data == "" || new_data == null) {

                    //empty the input tag
                    $('input[uid-input-model=' + $attrs.uidInputModel + ']').val("");

                    //empty the buffer variable
                    $scope.val_name = "";

                }
            });

            //To get Entered uid-input-model name
            var mask_model_name = $attrs.uidInputMask;

            var mask_model = $parse(mask_model_name);

            //watches for any change of data mask model from code
            $scope.$watch(mask_model, function (new_data, old_data) {
                var data = $scope.$eval($attrs.uidInputModel);
                if (data != null && data != undefined && data != "") {
                    if (new_data == true) {
                        $('input[uid-input-model=' + $attrs.uidInputModel + ']').val(data);
                    }
                    else {
                        var temp_value = "";

                        for (var i = 0; i < data.length; i++) {

                            var mask_length = $attrs.maskLength;
                            if (i < mask_length) {
                                temp_value = temp_value + 'X';
                            }
                            else {
                                temp_value = temp_value + data.charAt(i);
                            }

                        }
                        //adds modified data to input tag
                        $('input[uid-input-model=' + $attrs.uidInputModel + ']').val(temp_value);


                    }
                }
            });


        }



        var d = [[0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
        [1, 2, 3, 4, 0, 6, 7, 8, 9, 5],
        [2, 3, 4, 0, 1, 7, 8, 9, 5, 6],
        [3, 4, 0, 1, 2, 8, 9, 5, 6, 7],
        [4, 0, 1, 2, 3, 9, 5, 6, 7, 8],
        [5, 9, 8, 7, 6, 0, 4, 3, 2, 1],
        [6, 5, 9, 8, 7, 1, 0, 4, 3, 2],
        [7, 6, 5, 9, 8, 2, 1, 0, 4, 3],
        [8, 7, 6, 5, 9, 3, 2, 1, 0, 4],
        [9, 8, 7, 6, 5, 4, 3, 2, 1, 0]];


        // The permutation table
        var p = [
            [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
            [1, 5, 7, 6, 2, 8, 3, 0, 9, 4],
            [5, 8, 0, 3, 7, 9, 6, 1, 4, 2],
            [8, 9, 1, 6, 0, 4, 3, 5, 2, 7],
            [9, 4, 5, 3, 1, 2, 6, 8, 7, 0],
            [4, 2, 8, 6, 5, 7, 3, 9, 0, 1],
            [2, 7, 9, 3, 8, 0, 6, 4, 1, 5],
            [7, 0, 4, 6, 9, 1, 3, 2, 5, 8]];


        // The inverse table
        var inv = [0, 4, 3, 2, 1, 5, 6, 7, 8, 9];



        //  For a given number generates a Verhoeff digit

        //         Validates that an entered number is Verhoeff compliant.

        function validateVerhoeff(num) {

            if (num == 333333333333 || num == 666666666666 || num == 999999999999) {
                return false;
            }
            var cc;
            var c = 0;
            var myArray = StringToReversedIntArray(num);

            for (var i = 0; i < myArray.length; i++) {

                c = d[c][p[(i % 8)][myArray[i]]];

            }

            cc = c;
            if (cc == 0) {

                return true;

            }
            else {
                return false;


            }
        }



        /*
         * Converts a string to a reversed integer array.
         */
        function StringToReversedIntArray(num) {

            var myArray = [num.length];

            for (var i = 0; i < num.length; i++) {

                myArray[i] = (num.substring(i, i + 1));

            }

            myArray = Reverse(myArray);


            return myArray;

        }

        /*
         * Reverses an int array
         */
        function Reverse(myArray) {

            var reversed = [myArray.length];

            for (var i = 0; i < myArray.length; i++) {
                reversed[i] = myArray[myArray.length - (i + 1)];

            }

            return reversed;
        }


        return {
            restrict: 'A',
            Scope: true,
            controller: ['$scope', '$element', '$attrs', '$parse', aadhaarInput_function]
        };
    });

    app.directive('mobileInputModel', function () {

        function mobileInput_function($scope, $element, $attrs, $parse) {

            //creating a buffer variable
            $scope.val_name = "";

            //To get Entered uid-input-model name
            var input_model_name = $attrs.mobileInputModel;

            // Get the data model
            var model = $parse(input_model_name);

            //reads the mask length from attribute
            var mask_length = $attrs.maskLength;

            // For long press only one time take key value and stops from continues interation
            var flag = 0;

            //Defined special keys array
            var specialKeys = new Array();

            //Push backspace as a special key
            specialKeys.push(8);

            //Triggers when key pressed
            $element.bind("keypress", function (e) {

                //checks until the input tag length is less than 12 characters
                if ($('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val().length < 12) {

                    var keyCode = e.which ? e.which : e.keyCode;

                    //reads the entered key is number or not if number ret is tru
                    var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);

                    if (ret) {

                        //adds the number to  scope variable as a buffer which is used to store the number temperorily
                        $scope.val_name += String.fromCharCode(keyCode);
                    }

                    //assign the final data to the data model
                    model.assign($scope, $scope.val_name);

                    return ret;
                }
            });

            //Triggers before key released
            $element.bind("keyup", function (e) {

                //When first time keypress flag initializes to zero
                flag = 0;

                //checks the input tag length using data model
                if ($('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val().length == 10) {

                    //gets the data from the data model
                    var data = $scope.$eval($attrs.mobileInputModel);

                    //validates the data model using regex and if fails below condition executed to empty the input tag


                    /*
                    if (!check_mobile_number(data)) {
                        $('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val("");
                        model.assign($scope, "");
                        alert("Entered mobile number is invalid");
                    }
                    */


                }

                //function to fix android chrome bug which doesn't catch the entered input from key 
                var getKeyCode = function (str) {

                    //adds the key value to the scope varible which is further added to data model
                    $scope.val_name += str.substr(str.length - 1);

                    //if (str.length != str.length) {
                    //    $scope.val_name = $scope.val_name.substring(0, str.length);
                    //}

                };


                //for android chrome keycode fix only executes in chrome browser
                if (navigator.userAgent.match(/Android/i)) {

                    //reads the entered key value
                    var inputValue = this.value;
                    getKeyCode(inputValue);
                }


                //To get last entered character with complete string ex : 'XXXX5' 
                var a_val = $('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val();


                //if mask length is null replaces last character with 'X'
                if (mask_length == undefined || mask_length == null || mask_length == "") {
                    a_val = this.value.replace(/[^\d\X]/g, '').replace(/\d/g, 'X');

                    //adds modified data to input tag
                    $('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val(a_val);
                }
                else {
                    //if mask length is not null checks length replaces  with 'X' only until mentioned input length
                    //replaces with 'X' only until mentioned length
                    if (a_val.length <= mask_length) {
                        a_val = this.value.replace(/[^\d\X]/g, '').replace(/\d/g, 'X');
                    }

                    //adds modified data to input tag
                    $('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val(a_val);

                }


                //If length of input tag text is not matched with the data model trims the data model upto the length of input tag
                if (a_val.length != $scope.val_name.length) {
                    $scope.val_name = $scope.val_name.substring(0, a_val.length);
                }

                //assign the final data to the data model
                model.assign($scope, $scope.val_name);


            });

            //Triggers after key pressed
            $element.keydown(function (e) {

                //When first time keypress flag incremented and prevent second time entering when log press
                flag++;

                //reads key value
                var key = e.keyCode || e.charCode;

                //check wether key is backspace
                if (key != 8) {

                    //Prevents long press repeated iteration
                    if (flag > 1) {
                        //prevents key entering
                        e.preventDefault();
                    }
                }
                else {
                    if ($('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val().length > e.target.selectionStart) {
                        e.preventDefault();
                    }
                }

                //check wether key is backspace
                if (key == 46) {
                    e.preventDefault();
                    e.stopPropagation();
                }

                //check wether key is left arrow
                if (key == 37) {
                    e.preventDefault();
                }

                //check wether key is right arrow
                if (key == 39) {
                    e.preventDefault();
                }

            });

            //it disables drop function
            $element.bind("drop", function (e) {
                return false;
            });

            //it disables pasting the data function
            $element.bind("paste", function (e) {
                return false;
            });

            //watches for any change of data model from code
            $scope.$watch(model, function (new_data, old_data) {

                if (new_data == "" || new_data == null) {

                    //empty the input tag
                    $('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val("");

                    //empty the buffer variable
                    $scope.val_name = "";

                }
            });

            //To get Entered uid-input-model name
            var mask_model_name = $attrs.mobileInputMask;

            var mask_model = $parse(mask_model_name);

            //watches for any change of data mask model from code
            $scope.$watch(mask_model, function (new_data, old_data) {
                var data = $scope.$eval($attrs.mobileInputModel);
                if (data != null && data != undefined && data != "") {
                    if (new_data == true) {
                        $('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val(data);
                    }
                    else {
                        var temp_value = "";

                        for (var i = 0; i < data.length; i++) {

                            var mask_length = $attrs.maskLength;
                            if (i < mask_length) {
                                temp_value = temp_value + 'X';
                            }
                            else {
                                temp_value = temp_value + data.charAt(i);
                            }

                        }
                        //adds modified data to input tag
                        $('input[mobile-input-model=' + $attrs.mobileInputModel + ']').val(temp_value);


                    }
                }
            });

        }

        function check_mobile_number(data) {
            var response = data.match('[6-9]{1}[0-9]{9}');
            if (response) {
                return true;
            }
            else {
                return false;
            }
        }

        return {
            restrict: 'A',
            Scope: true,
            controller: ['$scope', '$element', '$attrs', '$parse', mobileInput_function]
        };
    });

})();