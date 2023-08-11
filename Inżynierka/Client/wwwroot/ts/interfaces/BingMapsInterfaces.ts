interface BingPin {
    color: string;
    location: Microsoft.Maps.Location;
}

interface bingPolygonOptions {
    fillColor: string;
    strokeColor: string;
    strokeThickness: number;
}

interface bingPolygon {
    rings: Microsoft.Maps.Location[][];
    options: bingPolygonOptions;
}