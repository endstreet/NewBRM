BRM_Utilities = {
    /**
     * @author Hendrix Viljon
     * Create a new browser window apart from active browser window.
     * @param {string} url              Navigation URL for the new browser window
     * @param {string} title            Title for the new browser window
     * @param {number} width            Width for the new browser window
     * @param {number} height           Height for the new browser window
     * @param {boolean} isTab           Is the new browser window a tab or is the new browser window a new window instance
     */
    newBrowserWindow: function (url, title, width, height, isTab) {
        var dualScreenLeft = window.screenLeft !== undefined ? window.screenLeft : screen.left;
        var dualScreenTop = window.screenTop !== undefined ? window.screenTop : screen.top;

        var calculatedWidth = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        var calculatedheight = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

        var left = calculatedWidth / 2 - width / 2 + dualScreenLeft;
        var top = calculatedheight / 2 - height / 2 + dualScreenTop;

        if (isTab === true) {
            $("<a>").attr("href", url).attr("target", "_blank")[0].click();
        }
        else {
            var newWindow = window.open(url, title, 'scrollbars=no, width=' + width + ', height=' + height + ', top=' + top + ', left=' + left);

            newWindow.parent = BRM_Utilities.setParentWindow();

            if (window.focus) {
                newWindow.focus();
            }
        }
    },

    /**
     * @author Hendrix Viljon
     * Show a modal from anywhere without the need to create in HTML
     * @param {any} options                     Options for Show Modal
     * @property {string} options.title         Modal Title
     * @property {string} options.body          Modal Content body
     * @property {string} options.remote        When setting the remote URL, the title, body and actions values will be ignored.
     * @property {string} options.backdrop      Modal
     * @property {string} options.size          Modal Size 'small' or 'large'
     * @property {string} options.onShow        Modal onShow: function(e){}
     * @property {string} options.onHide        Modal onHide: function(e){}
     * @property {string} options.actions       { label: 'Confirm', cssClass: 'btn-danger', onClick: function(e){ } }
     *
     */
    showModal: function (options) {
        options = $.extend({
            title: '',
            body: '',
            remote: false,
            backdrop: 'static',
            size: false,
            onShow: false,
            onHide: false,
            actions: false
        }, options);

        self.onShow = typeof options.onShow === 'function' ? options.onShow : function () { };
        self.onHide = typeof options.onHide === 'function' ? options.onHide : function () { };

        if (self.$modal === undefined) {
            self.$modal = $('<div class="modal fade"><div class="modal-dialog"><div class="modal-content"></div></div></div>').appendTo('body');
            self.$modal.on('shown.bs.modal', function (e) {
                self.onShow.call(this, e);
            });
            self.$modal.on('hidden.bs.modal', function (e) {
                self.onHide.call(this, e);
            });
        }

        var modalClass = {
            small: "modal-sm",
            large: "modal-lg"
        };

        self.$modal.data('bs.modal', false);
        self.$modal.find('.modal-dialog').removeClass().addClass('modal-dialog ' + (modalClass[options.size] || ''));
        self.$modal.find('.modal-content').html('<div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title">${title}</h4></div><div class="modal-body">${body}</div><div class="modal-footer"></div>'.replace('${title}', options.title).replace('${body}', options.body));

        var footer = self.$modal.find('.modal-footer');
        if (Object.prototype.toString.call(options.actions) === "[object Array]") {
            for (var i = 0, l = options.actions.length; i < l; i++) {
                options.actions[i].onClick = typeof options.actions[i].onClick === 'function' ? options.actions[i].onClick : function () { };
                $('<button type="button" class="btn ' + (options.actions[i].cssClass || '') + '">' + (options.actions[i].label || '{Label Missing!}') + '</button>').appendTo(footer).on('click', options.actions[i].onClick);
            }
        } else {
            $('<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>').appendTo(footer);
        }

        self.$modal.modal(options);
    },

    /**
     * @author Hendrix Viljon
     * Show the Parent window and close the active window
    */
    refocusWindowClose: function () {
        if (typeof originWindow !== 'undefined' && originWindow !== null) {
            originWindow.focus();
        }
        window.close();
    },

    /**
     * @author Hendrix Viljon
     * Sets the global parent window for called the origin window.
     * @param {object} newOrigin                     The new origin Window for
    */
    setParentWindow: function (newOrigin = undefined) {
        if (typeof originWindow === 'undefined' || originWindow === null) {
            originWindow = window;
        }

        if (typeof newOrigin !== 'undefined' && newOrigin !== null && typeof newOrigin !== 'object') {
            originWindow = newOrigin;
        }

        return originWindow;
    },

    /**
     * @author Hendrix Viljon
     * Gets the global parent window for called the origin window.
    */
    getOriginWindow: function () {
        return originWindow;
    }
};