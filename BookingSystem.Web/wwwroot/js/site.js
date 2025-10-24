// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Spinner functionality
var spinner = {
    show: function() {
        var loadingElement = document.querySelector('.loading');
        if (loadingElement) {
            loadingElement.style.display = 'block';
        }
    },
    hide: function() {
        var loadingElement = document.querySelector('.loading');
        if (loadingElement) {
            loadingElement.style.display = 'none';
        }
    }
};