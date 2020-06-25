class Applicant {
    get pensionNumber() {
        return this._pensionNumber;
    }

    set pensionNumber(value) {
        this._pensionNumber = value;
    }

    get childID() {
        return this._childID;
    }

    set childID(value) {
        this._childID = value;
    }

    get name() {
        return this._name;
    }

    set name(value) {
        this._name = value;
    }

    get surname() {
        return this._surname;
    }

    set surname(value) {
        this._surname = value;
    }

    get region() {
        return this._region;
    }

    set region(value) {
        this._region = value;
    }

    get grantType() {
        return this._grantType;
    }

    set grantType(value) {
        this._grantType = value;
    }

    get grantName() {
        return this._grantName;
    }

    set grantName(value) {
        this._grantName = value;
    }

    get applicationDate() {
        return this._applicationDate;
    }

    set applicationDate(value) {
        this._applicationDate = value;
    }

    get primaryStatus() {
        return this._primaryStatus;
    }

    set primaryStatus(value) {
        this._primaryStatus = value;
    }

    get secondaryStatus() {
        return this._secondaryStatus;
    }

    set secondaryStatus(value) {
        this._secondaryStatus = value;
    }

    get socStatus() {
        return this._socStatus;
    }

    set socStatus(value) {
        this._socStatus = value;
    }

    get statusDate() {
        return this._statusDate;
    }

    set statusDate(value) {
        this._statusDate = value;
    }

    get srdNumber() {
        return this._srdNumber;
    }

    set srdNumber(value) {
        this._srdNumber = value;
    }

    get brmBarcode() {
        return this._brmBarcode;
    }

    set brmBarcode(value) {
        this._brmBarcode = value;
    }

    get srd() {
        return this._srd;
    }

    set srd(value) {
        this._srd = value;
    }

    get clm() {
        return this._clm;
    }

    set clm(value) {
        this._clm = value;
    }

    get isRMC() {
        return this._isRMC;
    }

    set isRMC(value) {
        this._isRMC = value;
    }
}

class FileCoversheet {
    get applicant() {
        return this._applicant;
    }

    set applicant(value) {
        this._applicant = value;
    }

    get batching() {
        return this._batching;
    }

    set batching(value) {
        this._batching = value;
    }

    get boxNumber() {
        return this._boxNumber;
    }

    set boxNumber(value) {
        this._boxNumber = value;
    }

    get boxAudit() {
        return this._boxAudit;
    }

    set boxAudit(value) {
        this._boxAudit = value;
    }

    get transaction() {
        return this._transaction;
    }

    set transaction(value) {
        this._transaction = value;
    }
}

class MultiGrantMerge {
    get applicant() {
        return this._applicant;
    }

    set applicant(value) {
        this._applicant = value;
    }
}

class EnterBRM {
    get applicant() {
        return this._applicant;
    }

    set applicant(value) {
        this._applicant = value;
    }

    get batching() {
        return this._batching;
    }

    set batching(value) {
        this._batching = value;
    }
}