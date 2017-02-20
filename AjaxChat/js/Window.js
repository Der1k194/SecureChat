function InitializeWindow(windowDivID, titleDivID, cssClass, resizable,invisible) {
    if (windowDivID == null || cssClass == null) {
        alert("Invalid Arguments to function : InitializeWindow ");
    }
    else{
    
    windowDivSelector = "#" + windowDivID;
    titleDivSelector = "#" + titleDivID;
     
     // Set Appreance class to Window
    $(windowDivSelector).addClass(cssClass);
    
    //Make Window Resizable
    if (resizable) {
      //   windowDivResizeSelector = "#" + windowDivID + 'ResizeContent';
      //  windowMinHeight = $(windowDivSelector).height(); //limit minimum height resize
      ////  windowMinWidth = $(windowDivSelector).width();  //limit minimum width resize
     //   $(windowDivResizeSelector).resizable({ minHeight: windowMinHeight,
     //                                    minWidth: windowMinWidth,
     //                                   // animate: true,
     //                                    //autoHide: true,
     //                                    ghost: true,
     //                                    handles: 'n, e, s, w, se '                                         
     //                                 });
    }
  
  
    
    //Make Window Dragable
    if (titleDivID != null) {
        $(windowDivSelector).draggable({ opacity: 0.35, cursor: 'move', handle: titleDivSelector });
    }
    else {
        $(windowDivSelector).draggable({ opacity: 0.35, cursor: 'move' });
    }

   

    

    if (invisible) {
        $(windowDivSelector).hide();
    }
    
    }
    
}

//Nozel: Added to make Window visible at topleft position

function ShowWindowAt(windowDivID, animate, top, left) {

    if (windowDivID == null) {
        alert("Invalid Arguments to function : ShowWindow ");
    }
    else {

        windowDivSelector = "#" + windowDivID;
        if (animate) {
            $(windowDivSelector).fadeIn('slow', function() { $(windowDivSelector).reposition(top,left); })

        }
        else {
            $(windowDivSelector).show('fast', function() { $(windowDivSelector).reposition(top,left); });
        }
    }
}

function ShowWindow(windowDivID, animate) {

    if (windowDivID == null) {
        alert("Invalid Arguments to function : ShowWindow ");
    }
    else {

        windowDivSelector = "#" + windowDivID;
        if (animate) {
            $(windowDivSelector).center();
            $(windowDivSelector).fadeIn('slow')
            
        }
        else {
            $(windowDivSelector).center();
            $(windowDivSelector).show('fast');
        }
    }
}

//Nozel: Added to make Window disappear
function HideWindow(windowDivID, animate) {

    if (windowDivID == null) {
        alert("Invalid Arguments to function : ShowWindow ");
    }
    else {

        windowDivSelector = "#" + windowDivID;
        if (animate) {
            $(windowDivSelector).fadeOut('slow')
        }
        else {
            $(windowDivSelector).hide();
        }
    }
}


(function($) {
    $.fn.extend({
        center: function() {
            return this.each(function() {
                var top = ($(window).height() - $(this).outerHeight()) / 2;
                var left = ($(window).width() - $(this).outerWidth()) / 2;
                //$(this).css({ top: 0, left: 0 });
                $(this).css({ position: 'absolute', margin: 0, top: (top > 0 ? top : 0) + 'px', left: (left > 0 ? left : 0) + 'px' });
            });
        }
    });

    $.fn.extend({
        reposition: function(topPosition, leftPosition) {
            return this.each(function() {
                //$(this).css({ top: 0, left: 0 });
                $(this).css({ position: 'absolute', margin: 0, topPosition: (topPosition > 0 ? topPosition : 0) + 'px', leftPosition: (leftPosition > 0 ? leftPosition : 0) + 'px' });
            });
        }
    });
})(jQuery);