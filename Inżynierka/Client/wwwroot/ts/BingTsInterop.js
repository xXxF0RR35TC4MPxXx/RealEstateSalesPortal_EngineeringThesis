/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
/// <reference path="interfaces/BingMapsInterfaces.ts" />
var preciseBingMap;
var circleBingMap;
var PreciseBingMap = /** @class */ (function () {
    function PreciseBingMap(lat, long) {
        var _a;
        window.scroll({
            top: 0,
            left: 0,
            behavior: 'smooth'
        });
        this.map = new Microsoft.Maps.Map('#myMap', {
            center: new Microsoft.Maps.Location(lat, long),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 18
        });
        var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(lat, long), {
            icon: '<svg xmlns:svg="http://www.w3.org/2000/svg" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="25" height="40"><circle cx="12.5" cy="14.5" r="10" fill="{color}"/><image x="0" y="0" height="40" width="25" xlink:href="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABkAAAAnCAYAAAGNntMoAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxMAAAsTAQCanBgAAAPBSURBVEhLtZY/SGRXFMaHpE0bSJVkt8oKAf+CKytispGIjY1NcGXF/QM2wlYLgyCC+bPYpUhSCgZiJhapxEAKC0HE3cUtBLETFrExG2zMkPHm+5059/nmvTG+bHY+OL57vvOd79775s4dSzmMjY297cP6uKur67Pz8/NPFYOMrXJ0dBQIS5qis7PzgQ9LJfXesEEI4baSTzCLxIVJQzI6OmpJJGOO8wsbZJDwh4eHQRM8VzxV8w65FSK6u7vvYMvTqUb09vY2dkRoji7C0zqwSsPI4+NjGyj/KG7fuESRgnH8EeL7sXcEUero6PgclYhnyncYR44VfYwKMDYywvn8Xnp6egLhaUHI/8f19XU3rYMc3iUXwB4w1sq/9/gujuFroXaxDB2uJ7HhKqBDz5J++y9N6C1ZWFiIS/smhupfExp/pfgyrUugaadWV1c5YS+dMpDDU3cqj6GhIVaRgNxL/461tTVr4OnU1dAhuVmr1QJPp1oAbgm91i8Ud51qDr2ZWxMTE+H09NT2AqrVaoCj5rI6RJTTX1zXJzk1NJawhN3d3aSozyJ31gAGdqlx6FSoOk/DjXQ4bbADurS0VPi9m/bg4KBwg2m3trYKN5h2enraGrSn24TWzSVNJPcHgca0el2PeQMiLz3yPNGgpbE0MDBAHi/UZ4qnMUTvUENj4oi+vj743H7gqHnaCC7dLC69iIE+mPc43mnAebk5ZmdnXRoCY6cvB5eb65tfdFlwwE5OTgJhh60IJicnA+Hp1dBGbxKethD6lRzQcfiJ78H4+Hgol8uBW3Fubs5yeOrovKUY1PRocHDQ7jSgI9Vwo2ZBHR16+uj3Uh78tg8PDwd+c4HTBhlxltPBubaxSwxn539ZPz65/xU0e3l+fj5nDmQ0Kfpus4g1lyYQF/DD1wgNbk1NTcEXPxYFgB+++DPJ79vb2y2ZBF/8+Qo939/fb8kk+OLPTp5w88ZJ9Liud30tHeI+zIb4D7Ih/n0iToIv/vbd5rzzv6+EVcVD1R+kQ9z9bJ6Ke9k4+ftVVTr7HjXcF9rWrxsbG9RsRxL/4lGJodLP2aePVwj6lAd88CPPQVt7NDIyEvb29tAmn5EM/iREvYqh/A/CJWZOH/34OH05tM12CX+oVFjs1UCHnj63KA41Pl5cXHSr5qCOzlteDzJY29zcdMtGwFN36eujra3tnf7+/nB2dubWF4Cn7tL/B52UkZmZGbeugxzeJW8GMvx2ZWXFJuBJ7qU3C13fleXlZa7xilMtwVvt7e3v+rggSqV/ABNOzES/IWvuAAAAAElFTkSuQmCC"/></svg>',
            anchor: new Microsoft.Maps.Point(12, 39),
            color: "red"
        });
        (_a = this.map) === null || _a === void 0 ? void 0 : _a.entities.push(pushpin);
        window.scroll({
            top: 0,
            left: 0,
            behavior: 'smooth'
        });
    }
    return PreciseBingMap;
}());
var CircleBingMap = /** @class */ (function () {
    function CircleBingMap(lat, long, sitePolygon) {
        var _a;
        window.scroll({
            top: 0,
            left: 0,
            behavior: 'smooth'
        });
        this.map = new Microsoft.Maps.Map('#myMap', {
            center: new Microsoft.Maps.Location(lat, long),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 18
        });
        var polygon = new Microsoft.Maps.Polygon(sitePolygon.rings, { strokeColor: "red", strokeThickness: 2 });
        (_a = this.map) === null || _a === void 0 ? void 0 : _a.entities.push(polygon);
        window.scroll({
            top: 0,
            left: 0,
            behavior: 'smooth'
        });
    }
    return CircleBingMap;
}());
function loadCircleMap(lat, long, sitePolygon) {
    circleBingMap = new CircleBingMap(lat, long, JSON.parse(sitePolygon));
}
function loadPreciseMap(lat, long) {
    preciseBingMap = new PreciseBingMap(lat, long);
}
//# sourceMappingURL=BingTsInterop.js.map