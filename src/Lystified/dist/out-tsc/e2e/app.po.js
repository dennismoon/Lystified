"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var LystifiedPage = (function () {
    function LystifiedPage() {
    }
    LystifiedPage.prototype.navigateTo = function () {
        return protractor_1.browser.get('/');
    };
    LystifiedPage.prototype.getParagraphText = function () {
        return protractor_1.element(protractor_1.by.css('app-root h1')).getText();
    };
    return LystifiedPage;
}());
exports.LystifiedPage = LystifiedPage;
//# sourceMappingURL=app.po.js.map