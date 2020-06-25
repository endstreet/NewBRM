BRM_Business_Logic = {
    /**
     * @author Hendrix Viljon
     * Get the URL for the File Coversheet
     * @param {FileCoversheet} fileCoversheet File Coversheet
     * @returns {string}                Coversheet URL
     */
    getFileCoversheetURL: function (fileCoversheet) {
        this._fileCoversheet = fileCoversheet;
        var fileCoversheetURL = 'FileCover.aspx?PensionNo=' + fileCoversheet.applicant.pensionNumber
            + '&boxaudit=' + fileCoversheet.boxAudit
            + '&boxNo' + fileCoversheet.boxNumber
            + '&batching=' + (fileCoversheet.batching ? 'y' : 'n')
            + '&trans=' + fileCoversheet.transaction
            + '&BRM=' + fileCoversheet.applicant.brmBarcode
            + '&CLM=' + fileCoversheet.applicant.clm
            + '&gn=' + fileCoversheet.applicant.grantName
            + '&gt=' + fileCoversheet.applicant.grantType
            + '&appdate=' + fileCoversheet.applicant.applicationDate
            + '&SRDNo=' + fileCoversheet.applicant.srdNumber
            + '&tempBatch=' + fileCoversheet.applicant.isRMC
            + '&ChildID=' + fileCoversheet.applicant.childID;

        return fileCoversheetURL;
    },

    /**
     * @author Hendrix Viljon
     * Get the URL for the File Coversheet
     * @param {FileCoversheet} fileCoversheet File Coversheet
     */
    newFileCoversheetWindow: function (fileCoversheet) {
        var url = BRM_Business_Logic.getFileCoversheetURL(fileCoversheet);
        BRM_Utilities.newBrowserWindow(
            url
            , 'File Coversheet Print'
            , '720'
            , '400'
            , true
        );
    },

    /**
     * @author Hendrix Viljon
     * Get the URL for Multi Grant Merge
     * @param {MultiGrantMerge} multiGrantMerge    Multi Grant Merge
     * @returns {string}                Multi Grant Merge URL
     */
    getMultiGrantMergeURL: function (multiGrantMerge) {
        this._multiGrantMerge = multiGrantMerge;
        var multiGrantMergeURL = 'MultiGrantMerge.aspx?PensionNo=' + multiGrantMerge.applicant.pensionNumber
            + '&gt=' + multiGrantMerge.applicant.grantType
            + '&appdate=' + multiGrantMerge.applicant.applicationDate
            + '&ChildID=' + multiGrantMerge.applicant.childID;

        return multiGrantMergeURL;
    },

    /**
     * @author Hendrix Viljon
     * Get the URL for Multi Grant Merge
     * @param {MultiGrantMerge} multiGrantMerge    Multi Grant Merge
     */
    newMultiGrantMergeWindow: function (multiGrantMerge) {
        var url = BRM_Business_Logic.getMultiGrantMergeURL(multiGrantMerge);
        BRM_Utilities.newBrowserWindow(
            url
            , 'Multi-grant merge'
            , '590'
            , '420'
            , false
        );
    },

    /**
     * @author Hendrix Viljon
     * Get the URL for Enter BRM
     * @param {EnterBRM} enterBRM    Enter BRM
     * @returns {string}        Enter BRM URL
     */
    getEnterBRMURL: function (enterBRM) {
        var enterBRMURL = 'EnterBRM.aspx?PensionNo=' + enterBRM.applicant.pensionNumber
            + '&batching=' + (enterBRM.batching ? 'y' : 'n')
            + '&gt=' + enterBRM.applicant.grantType
            + '&gn=' + enterBRM.applicant.grantName
            + '&appdate=' + enterBRM.applicant.applicationDate
            + '&SRDNo=' + enterBRM.applicant.srdNumber
            + '&tempBatch=' + enterBRM.applicant.isRMC
            + '&ChildID=' + enterBRM.applicant.childID;

        return enterBRMURL;
    },

    /**
     * @author Hendrix Viljon
     * Opens a new Enter BRM Window
     * @param {EnterBRM} enterBRM    Enter BRM
     */
    newEnterBRMWindow: function (enterBRM) {
        var url = BRM_Business_Logic.getEnterBRMURL(enterBRM);
        BRM_Utilities.newBrowserWindow(
            url
            , 'Enter BRM barcode number'
            , '650'
            , '350'
            , false
        );
    },

    /**
     * @author Hendrix Viljon
     * Opens a Modal Window
     * @param {Applicant} selectedApplicant    Selected Applicant
     */
    BRMActionModal: function (selectedApplicant, fileCoversheet = undefined, enterBRM = undefined) {
        var brmActionModalOptions = {
            title: 'BRM Action',
            body: selectedApplicant.srdNumber === null || selectedApplicant.srdNumber === ''
                ? 'File Coversheet has already been generated for BRM Bar code \'' + selectedApplicant.brmBarcode + '\'. Please select an appropriate action.'
                : 'File Coversheet has already been generated for SRD Reference \'' + selectedApplicant.srdNumber + '\'. Please select an appropriate action.',
            //remote: false,
            //backdrop: 'static',
            size: 'large',
            //onShow: false,
            //onHide: false,
            actions: [
                {
                    label: 'Reprint Coversheet', cssClass: 'btn-primary', onClick: function (e) {
                        BRM_Business_Logic.newFileCoversheetWindow(fileCoversheet);
                        $(e.target).parents('.modal').modal('hide');
                    }
                },
                {
                    label: 'Add BRM File Number', cssClass: 'btn-success', onClick: function (e) {
                        BRM_Business_Logic.newEnterBRMWindow(enterBRM);
                        $(e.target).parents('.modal').modal('hide');
                    }
                },
                {
                    label: 'Cancel', cssClass: 'btn-info', onClick: function (e) {
                        $(e.target).parents('.modal').modal('hide');
                    }
                }
            ]
        };

        BRM_Utilities.showModal(brmActionModalOptions);
    }
};