/*
Theme Name: LESNAR HTML
Theme URI: http://megamindlab.com/lesnar
Description: Lesnar Html Theme
Author: megamindlab
Author URI: http://megamindlab.com
Version: 1.1
<?xml version="1.0" encoding="UTF-8"?>
*/
function initialize() {
	var myLatlng = new google.maps.LatLng( 34.04821476, -118.24844599 );
	var mapOptions = {
		zoom: 16,
		center: myLatlng,
		scrollwheel: false,
	};

	var map = new google.maps.Map( document.getElementById( 'map-canvas' ), mapOptions );

	var contentString = '<div class="map-content-info" style="float:left;width:240px;padding:10px 0px 10px 0px;overflow:hidden;text-align:center">' +
		'<div class="map-heading" style="float:left;width:100%;">' +
		'<h3 style="margin:0px;padding:3px 0px 3px 0px;">Lesnar</h3>' +
		'</div>' +
		'<div class="map-address" style="float:left;width:100%;">' +
		'<p style="margin:0px;padding:3px 0px 3px 0px;">Become a Legend Yourself.</p>' +
		'</div>' +
		'</div>';

	;

	var infowindow = new google.maps.InfoWindow( {
		content: contentString
	} );

	var image = 'assets/images/mml-marker.png';

	var marker = new google.maps.Marker( {
		position: myLatlng,
		map: map,
		icon: image
	} );

	google.maps.event.addListener( marker, 'click', function () {
		infowindow.open( map, marker );
	} );
}

google.maps.event.addDomListener( window, 'load', initialize );
