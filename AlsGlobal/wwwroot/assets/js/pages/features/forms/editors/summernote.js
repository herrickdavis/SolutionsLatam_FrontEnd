/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/*!******************************************************************!*\
  !*** ../demo1/src/js/pages/features/forms/editors/summernote.js ***!
  \******************************************************************/

// Class definition

var KTSummernoteDemo = function () {    
    // Private functions
    var demos = function () {
        $('.summernote').summernote({
            height: 150
        });
    }

    return {
        // public functions
        init: function() {
            demos(); 
        }
    };
}();

// Initialization
jQuery(document).ready(function() {
    KTSummernoteDemo.init();
});
/******/ })()
;
//# sourceMappingURL=summernote.js.map