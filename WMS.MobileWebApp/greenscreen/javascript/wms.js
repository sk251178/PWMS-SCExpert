var wms = {
    init: function () {
        if ($(document).width() < 500) {
	        //$('head').append('<link href="../greenscreen/css/mobile.css?v='+((new Date()).getTime())+'" rel="stylesheet" />');
			$('head').append('<link href="../greenscreen/css/mobile.css?v=2017022701" rel="stylesheet" />');
	    }
		
		if (ie6) {
			$(document).on('blur', 'input', function(){
				$(this).removeClass('input_focus');
			});
			$(document).on('focus', 'input', function(){
				$(this).addClass('input_focus');
			});
		}
		
		if (/LoginHQT\.aspx/.test(window.location.href)) {
			$('body').addClass('small_screen_two_columns');
		}
		
		if (/Login\.aspx/.test(window.location.href)) {
			// Changing the password field to show astreix
			//$('#Password').attr('type', 'password');
			// Signing out first
			if (window.location.hash != '#new-session') {
				window.location.href = '../m4nScreens/LogOffSession.aspx';
			}
		} else {
		    //commented for RWMS-2445 Start
			// Adding the logout button
		    //$('.header').prepend('<a class="logout" href="../m4nScreens/LogOffSession.aspx">Sign Out</a>');
		    //commented for RWMS-2445 End
		}
		
	    $(document).on('keypress', function (e) {
	        // ESC
			if (e.keyCode == '27') {
				if (!ie6) {
					window.localStorage.removeItem('last_focused_field');
				}
				window.history.back();
			}
			// F1
			if (e.keyCode == '112' && $('.actions .button:eq(0)').length > 0 && e.charCode == 0) {
				e.stopPropagation();
                e.preventDefault();
				if (!ie6) {
					window.localStorage.removeItem('last_focused_field');
				}
				$('.actions .button:eq(0) input').click();
			}
			// F2
			if (e.keyCode == '113' && $('.actions .button:eq(1)').length > 0 && e.charCode == 0) {
				e.stopPropagation();
                e.preventDefault();
				if (!ie6) {
					window.localStorage.removeItem('last_focused_field');
				}
				$('.actions .button:eq(1) input').click();
			}
			// F3
			if (e.keyCode == '114' && $('.actions .button:eq(2)').length > 0 && e.charCode == 0) {
				e.stopPropagation();
                e.preventDefault();
				if (!ie6) {
					window.localStorage.removeItem('last_focused_field');
				}
				$('.actions .button:eq(2) input').click();
			}
			// F4
			if (e.keyCode == '115' && $('.actions .button:eq(3)').length > 0 && e.charCode == 0) {
				e.stopPropagation();
                e.preventDefault();
				if (!ie6) {
					window.localStorage.removeItem('last_focused_field');
				}
				$('.actions .button:eq(3) input').click();
			}
		});
		
		// Last Field Focus
		$('input').focusin(function() {
			//console.log($('input:visible').index($('input:focus')));
			//window.location.href = '#'+$('input:visible').index($('input:focus'));
			if (!ie6 && $(':focus').attr('id')) {
				//window.localStorage.setItem('last_focused_field', $(':focus').attr('id'));
			}
		});
		if (!ie6) {
			$('.actions .button').click(function() {
				window.localStorage.removeItem('last_focused_field');
			});
		}
		setTimeout(function() {
			if (!ie6) {
				var last_focused_field_id = window.localStorage.getItem('last_focused_field');
			} else {
				var last_focused_field_id;
			}
			if (last_focused_field_id && $('#'+last_focused_field_id) && $('#'+last_focused_field_id).length > 0) {
				$('#'+last_focused_field_id).focus();
			} else if ($('.list a#btn1') && $('.list a#btn1').length > 0) {
				// Focusing on the first menu link
				$('.list a#btn1').focus();
			} else if ($('input,select').not('[type="hidden"]')) {
				$('input,select').not('[type="hidden"]').eq(0).focus();
			}
		}, 2000);
		
	},
	toggle_loader:function() {
		setTimeout(function() {
			$('body').css({display:'block'});
		}, 1500);
	}
};