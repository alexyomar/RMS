(function($)
  { 
//CUFON	

//    Cufon.replace('h2')('h3')('h4')('.banner em')
 
//CODA SLIDER

    $(window).bind("load", function() {
			$("div#slider1").codaSlider()
		});

//PORTFOLIO GRID
	
	$('.boxgrid.captionfull').hover(function(){
		$(".cover", this).stop().animate({top:'120px'},{queue:false,duration:160})
	}, function() {
		$(".cover", this).stop().animate({top:'160px'},{queue:false,duration:160})
	});


})(jQuery);