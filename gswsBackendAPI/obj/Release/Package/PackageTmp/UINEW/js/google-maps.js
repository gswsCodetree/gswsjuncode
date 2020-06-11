function initMap() {
  var map = new google.maps.Map(document.getElementById('map'), {
    zoom: 14,
    center: {lat: 16.5101619, lng: 80.6094416}
  });

  var ctaLayer = new google.maps.KmlLayer({
    url: 'http://65.19.149.210/apkml2/9.kml',
    map: map
  });
}


