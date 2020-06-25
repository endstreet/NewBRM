/*
*This function is used for displaying custom modal bootstrap messages
*/
var myECM;
myECM = myECM || (function () {
    var pleaseWaitDiv = $('<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:10%; overflow-y:visible;">' +
        '<div class="modal-dialog">' +
        '<div class="modal-content">' +
        '<div class="modal-header"><h3></h3></div>' +
        '<div class="modal-body">' +
        '</div>' +
        '<div class="modal-footer">' +
        '<button type="button" id="btnModalOK" style="display:none" class="btn btn-primary" data-dismiss="modal">OK</button>' +
        '<button type="button" id="btnModalClose" style="display:none" class="btn btn-primary" data-dismiss="modal">Close</button>' +
        '</div>' +
        '</div></div></div>');
    return {
        showPleaseWait: function (header, message, options) {
            // Assigning defaults
            if (typeof options === 'undefined' || options == null) {
                options = {};
            }
            if (typeof header === 'undefined' || header == null) {
                header = 'Loading...';
            }
            if (typeof message === 'undefined' || message == null) {
                message = '<div class="progress"> <div class="progress-bar progress-bar-striped progress-bar-animated" style="width:100%"></div></div>';
                pleaseWaitDiv.find('.modal-body').removeAttr('style');
                pleaseWaitDiv.find('.modal-footer').hide();
                pleaseWaitDiv.find('.modal-header').attr('class', 'modal-header').addClass('modal-header-center');
            } else {
                message = '<div style="width: 60px; float: left;"><img src="../Content/icons/alert.png" alt="" style="border-style: none; padding-right:10px; height:40px;"></div><div style="width: 450px; float: right; font-size:14px; margin-top:8px">' + message + '</div>';
                pleaseWaitDiv.find('.modal-body').attr('style', 'display:inline-block');
                pleaseWaitDiv.find('.modal-header').attr('class', 'modal-header').addClass('modal-header-left');
                pleaseWaitDiv.find('.modal-footer').show();
            }

            // Assigning defaults
            var settings = $.extend({
                dialogSize: 'sm',
                type: 'Alert',
                closeButton: '',
                closeFunction: '',
                OKButton: '',
                OKFunction: ''
            }, options);

            if (settings.type == 'Info') {
                message = message.replace('alert.png', 'info.png');
            }

            pleaseWaitDiv.find('#btnModalClose').hide();
            pleaseWaitDiv.find('#btnModalOK').hide();
            pleaseWaitDiv.find('#btnModalClose').off('click');
            pleaseWaitDiv.find('#btnModalOK').off('click');

            if (settings.closeButton != '') {
                pleaseWaitDiv.find('#btnModalClose').text(settings.closeButton);
                pleaseWaitDiv.find('#btnModalClose').show();
            }
            if (settings.closeFunction != '') {
                pleaseWaitDiv.find('#btnModalClose').off('click').on('click', settings.closeFunction);
            }
            if (settings.OKButton != '') {
                pleaseWaitDiv.find('#btnModalOK').text(settings.OKButton);
                pleaseWaitDiv.find('#btnModalOK').show();
            }
            if (settings.OKFunction != '') {
                pleaseWaitDiv.find('#btnModalOK').off('click').on('click', settings.OKFunction);
            }

            pleaseWaitDiv.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            pleaseWaitDiv.find('h3').text(header);
            pleaseWaitDiv.find('.modal-body').html(message);

            pleaseWaitDiv.modal();
        },
        hidePleaseWait: function () {
            pleaseWaitDiv.modal('hide');
        },
    };
})();

/*
*This function displays the remaining number of characters on an input field.
*Parameters:
*   elem - the input element that needs to be checked.
*   limit - the number of characters allowed
*   badgeID - the id of the badge element that will display the result
*/
function getFieldLengthRemaining(elem, limit, badgeID) {
    try {
        var val = document.getElementById(elem).value;
        if (val.length > limit) {
            val = val.substr(0, limit);
            document.getElementById(elem).value = val;
        }

        document.getElementById(badgeID).innerHTML = (limit - val.length);
    }
    catch (Exception) {
    }
}