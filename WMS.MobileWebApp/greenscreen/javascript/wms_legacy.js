var wms_legacy = {
    init:function () {
		
		$('body').addClass('small_screen_two_columns');
		
		$('link[href*="Styles.css"]').remove();
		
        if ($(document).width() < 500) {
	        //$('head').append('<link href="../greenscreen/css/mobile.css?v='+((new Date()).getTime())+'" rel="stylesheet" />');
			$('head').append('<link href="../greenscreen/css/mobile.css?v=2017022701" rel="stylesheet" />');
			wms_legacy.mobile = true;
	    }
		
		if (ie6) {
			$(document).on('blur', 'input', function(){
				$(this).removeClass('input_focus');
			});
			$(document).on('focus', 'input', function(){
				$(this).addClass('input_focus');
			});
		}
		
		
		// Template
		$('body > form:eq(0)').append('\
			<div class="header"></div>\
			<div class="content"></div>\
		');
		
		// Header
		if (wms_legacy.mobile && $('.RDTTitleCell').length > 0) {
			$('.header').html($('.RDTTitleLabel').html()+'</span>');
			$('.RDTTitleCell').remove();
		} else if (!wms_legacy.mobile && $('.RDTTitleCell').length > 0) {
			$('.header').html('<a class="main" href="../Screens/Main.aspx">Main</a> &rsaquo; <span>'+$('.RDTTitleLabel').html()+'</span>');
			$('.RDTTitleCell').remove();
		} else if ($('#DO1_ContentCell a#btn1, #NavigationInterface1 a#btn1').length > 0) {
			$('.header').html('Dashboard');
		} else {
			$('.header').html($('title').html());
		}
		
		if (/Login\.aspx/.test(window.location.href)) {
			// Changing the password field to show astreix
			$('#Password').attr('type', 'password');
			// Signing out first
			if (window.location.hash != '#new-session') {
				window.location.href = '../m4nScreens/LogOffSession.aspx';
			}
		} else {
            //commented for RWMS-2445 Start
			// Adding the logout button
			//$('.header').prepend('<a class="logout" href="../m4nScreens/LogOffSession.aspx">Sign Out</a>');
			// Adding the back button
		    //$('.header').prepend('<a class="back" href="javascript:window.history.back()">Back</a>');
		    //commented for RWMS-2445 End
		}
		
		// Content
		if ($('#DO1_ContentCell a#btn1, #NavigationInterface1 a#btn1').length > 0) {
			
			// Dashboards
			$('.content').before('<div class="list"></div>');
			$('#DO1_ContentCell, #NavigationInterface1').find('a').appendTo('.list');
			
			$('.list a > img').remove();
			$('.list a > br').remove();
			if ($('.list a').length > 8) {
				$('.list').addClass('condensed');
			}
			
			// Dashboard Action Buttons
			$('.content').append('<div class="actions"></div>');
			$('.ActionBtnClass').closest('table').find('input').each(function() {
				if ($(this).attr('type') == 'hidden') {
					$('.actions').append($(this)[0].outerHTML);
				} else {
					$('.actions').append('<div class="button">'+($(this)[0].outerHTML)+'</div>');
				}
			});
			
			$('#DO1_ContentCell, #NavigationInterface1').remove();
			
		} else {
			
			// Forms
			$('.content').html('<div class="table"></div>');

			// Legacy Input Fields
			$('#DO1_ContentCell > table.RDTContentTable > tbody > tr > td').each(function() {
				if ($(this).find('td:eq(1)').html() && $(this).find('td:eq(0)').html() && $(this).find('td:eq(0)').html().length > 5) {
					$('.content .table').append('\
						<div class="field">\
							<div class="container">\
								'+$(this)[0].outerHTML+'\
							</div>\
						</div>\
					');
				}
				if ($(this).find('td').length >= 3) {
					$('.content .table .field').last().find('.container > table').addClass('condensed');
				}
				if ($(this).find('table > tbody > tr > td:eq(2) > p.label').length > 0) {
					$('.content .table .field').last().addClass('full');
				}
			});
			
			//alert($('span#DO1_AdditionalAttributeslbl')[0].outerHTML);
			
			// Converting the legacy select-box buttons
			$('.content .table input.button').each(function() {
				if ($(this).val() == 'DOWN') {
					$(this).val('>');
				}
				if ($(this).val() == 'UP') {
					$(this).val('<');
					$(this).closest('.field').addClass('legacy_select_box');
				}
			})
			
			// If we have many fields - we use 3 columns
			if ($('.content .table .field').length > 9) {
				$('.content .table .field').addClass('very_condensed');
			} else if ($('.content .table .field').length > 6) {
				$('.content .table .field').addClass('condensed');
			}
			
			/*
			$('#DO1_ContentCell > table.RDTContentTable > tbody > tr > td > table > tbody > tr > td > span').each(function() {
				$('.content .table').append('\
					<div class="field">\
						<div class="container">\
							'+($(this).closest('tr').prev().find('p').length > 0 ? $(this).parent().prev().find('p')[0].outerHTML : '')+'\
							'+($(this)[0].outerHTML)+'\
						</div>\
					</div>\
				');
			});
			// Legacy Select Boxes
			$('#DO1_ContentCell').find('td > table').each(function() {
				$('.content .table').append('\
					<div class="field">\
						<div class="container">\
							'+($(this).closest('tr').prev().find('p').length > 0 ? $(this).parent().prev().find('p')[0].outerHTML : '')+'\
							<div class="legacy_select_box">'+($(this)[0].outerHTML)+'</div>\
						</div>\
					</div>\
				');
			});
			*/
			
			// Form Action Buttons
			$('.content').append('<div class="actions"></div>');
			$('#DO1_ActionBar input').each(function() {
				if ($(this).attr('type') == 'hidden') {
					$('.actions').append($(this)[0].outerHTML);
				} else {
					$('.actions').append('<div class="button">'+($(this)[0].outerHTML)+'</div>');
				}
			});
			
		}
		
		var keystrokes_timer;
		var keystrokes_input = '';
		$(document).on('keypress', function (e) {
			if ($('.list a#btn1').length > 0) {
				// Dashboard Keystrokes
				var c = String.fromCharCode(e.keyCode);
				if (/\d/.test(c)) {
					keystrokes_input = keystrokes_input + c;
					
					clearTimeout(keystrokes_timer);
					keystrokes_timer = setTimeout(function() {
						if (!ie6) {
							window.localStorage.removeItem('last_focused_field');
						}
						window.location.href = $('#btn'+keystrokes_input).attr('href');
						keystrokes_input = '';
					}, 1000);
				}
			}
		    // ESC
			var isChar = e.charCode;
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
		
		// Removing the old template
		$('#DO1_MainTable').remove();
		
	},
	toggle_loader:function() {
		setTimeout(function() {
			$('body').css({display:'block'});
		}, 1500);
	}
};