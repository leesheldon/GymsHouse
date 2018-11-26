/*
Theme Name: LESNAR HTML
Theme URI: http://megamindlab.com/lesnar
Description: Lesnar Html Theme
Author: megamindlab
Author URI: http://megamindlab.com
Version: 1.1
<?xml version="1.0" encoding="UTF-8"?>
*/
"use strict";

// preloader
$( window )
	.load( function () {
		$( '#preloader' )
			.fadeOut( 'slow', function () {
				$( this )
					.remove();
			} );
	} );

// owl carousel one
function owlCarOne() {
	if ( $( "#owl-demo" )
		.length ) {
		var owl = $( "#owl-demo" );

		owl.owlCarousel( {

			itemsCustom: [
				[ 0, 1 ],
				[ 450, 1 ],
				[ 600, 2 ],
				[ 700, 2 ],
				[ 1000, 4 ],
				[ 1200, 4 ],
				[ 1400, 4 ],
				[ 1600, 4 ]
			],
			navigation: true

		} );
	};
}
// owl carousel two
function owlCarTwo() {
	if ( $( "#owl-team" )
		.length ) {
		var owl = $( "#owl-team" );

		owl.owlCarousel( {

			itemsCustom: [
				[ 0, 1 ],
				[ 450, 1 ],
				[ 600, 2 ],
				[ 700, 2 ],
				[ 1000, 4 ],
				[ 1200, 4 ],
				[ 1400, 4 ],
				[ 1600, 4 ]
			],
			navigation: true

		} );
	};
}
// parallax configuration
function parallaxConfig() {
	$( window )
		.stellar( {
			horizontalScrolling: false
		} );
}
// search toggler
function searchToggler() {
	if ( $( '#mml-search-icon' )
		.length ) {
		$( '#mml-search-icon' )
			.on( 'click', function () {
				$( '.header-serarch-area' )
					.slideToggle();
				$( '.header-cart-area' )
					.slideUp();
				return false;
			} );
	};
}
// cart toggler
function cartToggler() {
	if ( $( '#mml-cart-icon' )
		.length ) {
		$( '#mml-cart-icon' )
			.on( 'click', function () {
				$( '.header-cart-area' )
					.slideToggle();
				$( '.header-serarch-area' )
					.slideUp();
				return false;
			} );
	};
}
// gallery filter
function galleryFilter() {
	if ( $( '#grid' )
		.length ) {
		/* initialize shuffle plugin */
		var $grid = $( '#grid' );

		$grid.mixItUp();
	};

}
// countdown timer
function countDownTimer() {
	var newYear = new Date();
	newYear = new Date( 2016, 12 - 1, 25 );
	$( '.defaultCountdown' )
		.countdown( {
			until: newYear,
			format: 'DHMS'
		} );
}

// revolution slider
function revolutionSliderActiver() {
	if ( $( '.rev_slider_wrapper #slider1' )
		.length ) {
		$( "#slider1" )
			.revolution( {
				sliderType: "standard",
				sliderLayout: "auto",
				delay: 5000,
				navigation: {
					arrows: {
						enable: true
					}
				},
				gridwidth: 1170,
				gridheight: 705
			} );
	};
}

function contactFormValidation() {
	if ( $( '.contact-form' )
		.length ) {
		$( '.contact-form' )
			.validate( { // initialize the plugin
				rules: {
					name: {
						required: true
					},
					email: {
						required: true,
						email: true
					},
					message: {
						required: true
					},
					subject: {
						required: true
					}
				},
				submitHandler: function ( form ) {
					// sending value with ajax request
					$.post( $( form )
						.attr( 'action' ), $( form )
						.serialize(),
						function ( response ) {
							$( form )
								.parent( 'div' )
								.append( response );
							$( form )
								.find( 'input[type="text"]' )
								.val( '' );
							$( form )
								.find( 'input[type="email"]' )
								.val( '' );
							$( form )
								.find( 'textarea' )
								.val( '' );
						} );
					return false;
				}
			} );
	}
}




// Dom Ready Function
jQuery( document )
	.on( 'ready', function () {
		( function ( $ ) {
			// add your functions
			revolutionSliderActiver();
			countDownTimer();
			galleryFilter();
			searchToggler();
			parallaxConfig();
			cartToggler();
			owlCarOne();
			owlCarTwo();
			contactFormValidation();
		} )( jQuery );
	} );
