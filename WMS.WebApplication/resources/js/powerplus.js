var powerplus = {
	renderLogin:function() {
		if ($('#LoginBox1').length > 0) {
			var new_login_html = $('#LoginBox1').html().replace(/td>/g, 'div>').replace(/<\/?(tr|tbody)>/g, '').replace(/<table[^>]*>/g, '').replace(/<\/table>/g, '');
			$('#LoginBox1').html(new_login_html);
			setTimeout(function() {
				$('#LoginBox1_user_tb').focus();
			}, 100);
		}
		if ($('select#lstWareHouse').length > 0) {
			$('select#lstWareHouse').removeAttr('size');
		}
	},
	renderHeader:function() {
		$('.TopInfo').find('nowrap br').after('&nbsp;&nbsp;');
		$('.TopInfo').find('nowrap br').remove();
		$('.TopInfo').find('.TopLinks:eq(0)').html('<span class="icon-user" title="User Details"></span>');
		$('.TopInfo').find('.TopLinks:eq(1)').html('<span class="icon-log-out" title="Log out"></span>');
		
		$('#Screen1 > table:eq(2)').addClass('header');
		
		$('#Navbar .rmLink.rmImageOnly.rmRootLink img').remove();
		
		$('body > form > div:not(#calendar):not(#selectMonth):not(#selectYear):not(#RWM):not(.LinkedTextBox_Container):eq(0)').addClass('page');
		$('.ActionBar').parent().closest('table').closest('table').addClass('actions_bar');
		$('.actions_bar').not(':eq(0)').addClass('not_fixed');
		
		$('#Navbar a > img').each(function() {
			$(this).parent().html($(this).parent().html().replace('&nbsp;', ''));
		});
		
		$('.page .RadGrid_Default').css({width:$(window).width()});
		$('.page .DockingLayoutTable').css({position:'relative', left:'10px',width:($(window).width()-10),height:($(window).height()-120)});
		$('.page .DockingLayoutTable .RadGrid_Default').css({width:'auto'});
		
		$('img[src*="m4nSkins/Default/Images/LeftBanner.jpg"]').css({cursor:'pointer'});
		$('img[src*="m4nSkins/Default/Images/LeftBanner.jpg"]').parent().on('click', function() {
			window.location='../Screens/Main.aspx';
		});
		
		//$('#DTC_TS td[style*="border-bottom"]').remove();
		
		$('.CheckboxInGrid').parent().css({'text-align':'center'});
		
		// Dashboard Adjustments
		$('.RadDock.RadDock_Default:eq(0)').find('em:eq(0)').html('Warehouse Activity');
		$('.RadDock.RadDock_Default:eq(2)').find('em:eq(0)').html('Work Region Management');
		$('.RadDock.RadDock_Default:eq(3)').find('em:eq(0)').html('Tasks to End of Shift');
		$('.RadDock.RadDock_Default:eq(4)').find('em:eq(0)').html('PO Expected Cases');
		
		$('.RadDock.RadDock_Default:eq(5)').hide();
		
		$('.RadDock.RadDock_Default:eq(7)').find('em:eq(0)').html('Tasks in Process');
		$('.RadDock.RadDock_Default:eq(8)').find('em:eq(0)').html('Outbound Statuses');
		
		$('head').append('\
			<style type="text/css">\
				#ACCONTROL1__ctl0_MyList_PR_WRSS_C__ctl0_TableEditorGrid_ctl0_Grid th:nth-child(5) {display:none}\
				#ACCONTROL1__ctl0_MyList_PR_WRSS_C__ctl0_TableEditorGrid_ctl0_Grid td:nth-child(5) {display:none}\
			</style>\
		');
		
		$('#ACCONTROL1__ctl0_MyList_PR_TTEOS').remove();
		
		$('#ACCONTROL1__ctl0_MyList_PR_EMP_C').css({height:'655px'});
		if (navigator.userAgent.match(/Trident|Edge/i)) {
			$('#ACCONTROL1__ctl0_MyList_PR_EPO_C__ctl0').css({height:'498px'});
		} else {
			$('#ACCONTROL1__ctl0_MyList_PR_EPO_C__ctl0').css({height:'490px'});
		}
		$('#ACCONTROL1__ctl0_MyList_IC_C').css({height:'305px'});
		$('#ACCONTROL1__ctl0_MyList_PR_AS_C').css({height:'305px'});
		
		
		// Lastscreen button is broken - changing it with "back"
		$('a[href="LastScreen"]').attr('href', 'javascript:window.history.back()');
		
	},
	renderIcons:function() {
		$('#CloseButtonRadWindowClass3001').before('<span class="icon-cross" onclick="'+$('#CloseButtonRadWindowClass3001').attr('onclick')+'"></span>');
		$('#CloseButtonRadWindowClass3001').remove();
		
		$('.RadDockableObjectCommandButton[title=Close]').html('<span class="icon-cross"></span>');
		$('.RadDockableObjectCommandButton[title=Collapse]').html('<span class="icon-minus"></span>');
		$('.RadDockableObjectCommandButton[title=Expand]').html('<span class="icon-plus"></span>');
		
		//$('input[name*="DATE"]').addClass('timestamp');
	},
	renderTypeAhead:function() {
		if (navigator.userAgent.match(/Trident|Edge/i)) {
			return;
		}
		$('span > table > tbody > tr > td > span[classname="Made4Net.WebControls.DisplayTypes.TypeAhead"]').each(function() {
			$(this).find('> input').attr('type', 'text');
			$(this).find('> input').attr('name', $(this).find('> input').attr('name').replace(':IText', ':TypeAheadBox'));
			var existing_value = $(this).find('> table input:visible').val();
			var decoded_script = powerplus.htmlspecialchars_decode($(this).find('script').html());
			var matches;
			
			matches = decoded_script.match(/data_provider_url="([^"]+)"/i);
			var url = matches && matches[1] ? matches[1] : null;
			
			var request = {};

			matches = window.location.href.match(/sc=([^&]+)/i);
			request.urlparam_sc = matches && matches[1] ? matches[1] : null;
			
			matches = decoded_script.match(/text_field="([^"]+)"/i);
			request.p_search_field = matches && matches[1] ? matches[1] : null;
			request.p_order_by = matches && matches[1] ? matches[1] : null;
			
			var p_fields = [];
			var p_fields_exists = {};
			matches = decoded_script.match(/value_field="([^"]+)"/i);
			if (matches && matches[1]) {
				p_fields.push(matches[1]);
				p_fields_exists[matches[1]] = true;
			}
			matches = decoded_script.match(/text_field="([^"]+)"/i);
			if (matches && matches[1] && !p_fields_exists[matches[1]]) {
				p_fields.push(matches[1]);
				p_fields_exists[matches[1]] = true;
			}
			matches = decoded_script.match(/extra_fields="([^"]+)"/i);
			if (matches && matches[1] && !p_fields_exists[matches[1]]) {
				p_fields.push(matches[1]);
				p_fields_exists[matches[1]] = true;
			}
			if (p_fields.length > 0) {
				request.p_fields = p_fields.join(',');
			}

			//matches = decoded_script.match(/extra_fields="([^"]+)"/i);
			//request.p_fields = matches && matches[1] ? matches[1] : null;
			
			matches = decoded_script.match(/table="([^"]+)"/i);
			request.p_table = matches && matches[1] ? matches[1] : null;

			matches = decoded_script.match(/record_limit="([^"]+)"/i);
			request.p_record_limit = matches && matches[1] ? matches[1] : null;

			request.p_rand = '';
			request.p_connection = '';
			request.p_search_value = '';

			matches = decoded_script.match(/filter="([^"]+)"/i);
			// Note we're converting the decoding the html entities
			request.p_filter = ''; //matches && matches[1] ? $('<textarea />').html(matches[1]).text() : null;
 			
			matches = decoded_script.match(/name="([^\:]+)\:/i);
 			request.p_parent_te = matches && matches[1] ? matches[1] : null;

			//request.p_search_value;
			
			$(this).find('> table').remove();
			$(this).find('> script').remove();
			
			var element = $(this);
			
			if (existing_value) {
				element.find('> input[type="text"]').val(existing_value);
			}
			
			$.post(url, request, function(response) {
				//console.log(element, request, {response});
				var xml = $.parseXML(response);
				response = [];
				$.each($(xml).find('Record'), function(i, record) {
					if ($(record).children().length > 1) {
						// We have multiple sub-records
						var text = [];
						$.each($(record).children(), function(i, sub_record) {
							text.push($(sub_record).text());
						});
						response.push({value:text[0], label:(text[1]+' ('+text[0]+')')});
					} else {
						// We have a single record value
						response.push($(record).text()); 
					}
				});
				element.find('> input[type="text"]').autocomplete({
					source:response,
					select:function( event, ui ) {
						if (ui.item.value) {
							element.find('> input').val(ui.item.value.trim());
						}
						return false;
					},
					blur: function( event, ui ) {
						// Selected an item, nothing to do
						if ( ui.item ) {
						  return;
						} else {
							alert('Please select an item from the list.');
						}
					}
				})._renderItem = function(ul, item) {
				  return $('<li>').append(item.label).appendTo(ul);
				};
			}, 'text');
			
		});
	},
	htmlspecialchars:function(phrase) {
		if (phrase) {
			return phrase.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&#039;");
		} else {
			return '';
		}
	},
	htmlspecialchars_decode:function(phrase) {
		if (phrase) {
			return phrase.replace(/&amp;/g, "&").replace(/&lt;/g, "<").replace(/&gt;/g, ">").replace(/&quot;/g, '"').replace(/&#039;/g, "'");
		} else {
			return '';
		}
	},
}