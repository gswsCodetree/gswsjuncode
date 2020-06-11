$(document).on('keyup change input', '[chars]', function (event) {

	var $elem = $(this),
		value = $elem.val(),
		regReplace,
		preset = {
			'only-numbers': '0-9',
			'numbers': '0-9\\s',
			'only-letters': 'A-Za-z',
			'letters': 'A-Za-z\\s',
			'email': '\\wÑñ@._\\-',
			'alpha-numeric': '\\w\\s',
			'latin-alpha-numeric': '\\w\\sÑñáéíóúüÁÉÍÓÚÜ'
		},
		filter = preset[$elem.attr('chars')] || $elem.attr('chars');

	regReplace = new RegExp('[^' + filter + ']', 'ig');
	$elem.val(value.replace(regReplace, ''));

});