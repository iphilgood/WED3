# Graphics

## Goals

Die Teilnehmer können ...

- mit Canvas ein simples Spiel erstellen
- den Unterschied zwischen Raster- und Vektor-Grafiken aufzeigen
- Vektor-Code interpretieren
- die Grundelemente von Vektor-Grafiken anwenden
- Mit SVG einfache Vektor-Grafiken erstellen
- den Viewport vom SVG an Beispielen erklären
- entscheiden ob SVG, Canvas oder WebGL für Ihr Projekt in Frage kommt
- Canvas Code interpretieren
- Transition / Animationen interpretieren
- die Grundkonzepte vom 3D-Programmieren erklären

## Vektor vs. Raster Graphics

* Raster Grafik
  * Bei Raster Grafiken wird für jeden Punkt die Farbe abgelegt (z.B. RGB)
  * Bei anderen Auflösungen wird die Grafik vergrössert/verkleinert
    * Je nach Algorithmus besser oder schlechter
* Vektor Grafik
  * Alle Vektor basierten Grafiken können durch Mathematik ausgedrückt werden
    * z.B. Kreis bei X = 10 und Y = 10 mit Radius = 5
  * Daraus folgt, dass Grafiken zu jeder beliebigen Auflösung ohne Informationsverlust transformiert werden können

## Vector Overview

* SVG (Scalable Vector Graphics)
  * Features: Sehr flexibel, CSS & JS, Event-Handling, Sehr gute Tools
  * Geeignet für: Animationen, Grafiken, Charts und einfache Spiele
  * Nachteile: Performance
* Canvas
  * Features: Bitmap Painting
  * Geeignet für: Viele Objekte
  * Vorteile: Performance, JavaScript, Browser Support, Pixel Support, Basis für WebGL
  * Nachteile: Event Handling, Accessibility
  * Beispiel: https://www.bing.com/maps (Nur Desktop)
* WebGL
  * Features: 3D, Shaders, GPU
  * Geeignet für: «echte» Spiele
  * Nachteile: Sehr komplex
  * Beispiele: http://hexgl.bkcore.com/play, https://www.google.ch/maps (nur Desktop)

## Basis Vektor Elemente

* Rectangle
* Circle
  * Ellipse
* Line
* Polyline
* Polygon

[More Information](https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Basic_Shapes)

### Polyline vs. Polygon

```html
<svg viewBox="0 0 200 200">
  <polyline points="0 0 200 0 200 100 100 200 0 200" 
            stroke="red" 
            fill="blue" 
            stroke-width="6" />
</svg>

<svg viewBox="0 0 200 200">
  <polygon points="0 0 200 0 200 100 100 200 0 200" 
           stroke="red" 
           fill="blue" 
           stroke-width="6" />
</svg>
```

* Eine Polyline ist ein Zug von X-Y Punkten, welche mit einer Linie verbunden werden
  * Start- und Endpunkte können unterschiedlich sein
* Das Polygon stellt sicher, dass die Geometrie abgeschlossen ist

### Path

* Das mächtigste Werkzeug von Vektor-Grafiken
* Paths ermöglichen es komplexe Figuren zu definieren

```html
<path d="M 100 100 L 300 100 L 200 300 z" 
      fill="orange" 
      stroke="black" 
      stroke-width="3" />
```

| Type            | Desciption                               |
| --------------- | ---------------------------------------- |
| **M** x,y       | Move to the absolute coordinates x,y     |
| **m** x,y       | Move to the right x and down y (or left and up if negative values) |
| **L** x,y       | Draw a straight line to the absolute coordinates x,y |
| **l** x,y       | Draw a straight line to a point that is relatively right x and down y (or left and up if negative values) |
| **H** x         | Draw a line horizontally to the exact coordinate x |
| **h** x         | Draw a line horizontally relatively to the right x (or to the left if a negative value) |
| **V** y         | Draw a line vertically to the exact coordinate y |
| **v** y         | Draw a line vertically relatively down y (or up if a negative value) |
| **Z**(or **z**) | Draw a straight line back to the start of the path |

#### Advanced path definitions

| Type                                    | Description                              |
| --------------------------------------- | ---------------------------------------- |
| **C** cX1,cY1 cX2,cY2 eX,eY             | Draw a bezier curve based on **two** bezier control points and end at specified coordinates |
| **c**                                   | Same with all relative values            |
| **S **cX2,cY2 eX,eY                     | Basically a C command that assumes the first bezier control point is a reflection of the last bezier point used in the previous S or C command |
| **s**                                   | Same with all relative values            |
| **Q** cX,cY eX,eY                       | Draw a bezier curve based a **single** bezier control point and end at specified coordinates |
| **q**                                   | Same with all relative values            |
| **T** eX,eY                             | Basically a Q command that assumes the first bezier control point is a reflection of the last bezier point used in the previous Q or T command |
| **t**                                   | Same with all relative values            |
| **A **rX,rY rotation, arc, sweep, eX,eY | Draw an arc that is based on the curve an oval makes. First define the width and height of the oval. Then the rotation of the oval. Along with the end point, this makes two possible ovals. So the arc and sweep are either 0 or 1 and determine which oval and which path it will take. |
| **a**                                   | Same with relative values for eX,eY      |

[Source](https://css-tricks.com/svg-path-syntax-illustrated-guide/)

## SVG

* [Generelle Informationen](https://developer.mozilla.org/en-US/docs/Web/SVG)
* [Informationen über das DOM Element](https://developer.mozilla.org/en-US/docs/Web/SVG/Element/svg)
* Können inline im HTML definiert werden
* Können als Bilder oder Objekte eingebunden werden
  * Als Bild verlieren sie die komplette Interaktionsmöglichkeiten

### Grösse von einem SVG

* Ein SVG ist ein selbständiges Dokument
* Ein SVG hat ein eigenes Koordinatensystem
* Der Browser kenn die Grösse vom Inhalt nicht
  * Default Grösse von einem SVG ist **300px auf 150px** (Kein Standard)
  * => SVG muss die Grösse angeben

#### Variante 1: Width und Height Attribute setzen

```html
<svg width="200" height"200">
  <rect x="0" y="0" width="200" height="200"></rect>
</svg>
```

Dies kann jedoch zu [Problemen](https://css-tricks.com/scale-svg/#article-header-id-2) führen.

#### Variante 2: ViewBox

```html
<svg viewBox="0 0 200 200">
  <rect x="0" y="0" width="200" height="200"></rect>
</svg>
```

### ViewBox

* Gibt das Verhältnis vom SVG an
* Definiert den 0/0 Punkt innerhalb vom SVG
* Definiert den sichtbaren Bereich
* Wert wird durch Komma oder Whitespace getrennt
  * `x y width height`
* Kann mit JS angepasst werden

### [preserveAspectRatio](https://developer.mozilla.org/en/docs/Web/SVG/Attribute/preserveAspectRatio)

* Nur relevant, wenn die viewBox definiert ist
* Gibt das Verhalten bei einem Verhältnis-missmatch an
  * Zentrieren, verzerren, links aussrichten

```
preserveAspectRatio="none"
preserveAspectRatio="xMaxYMin"
preserveAspectRatio="xMidYMid slice"
preserveAspectRatio="xMidYMid meet"
```

## Canvas

```html
<body>
  <canvas id="painting">
    <!-- HTML Fallback falls Canvas nicht unterstützt wird -->
    Hello World Demo
  </canvas>
  
  <script>
    const painting = document.getElementById("painting");
    // Check ob Canvas unterstützt wird
    if (painting.getContext) {
      var context = painting.getContext("2d");
      context.fillRect(0, 0, 300, 150);
    }
  </script>
</body>
```

### Shapes

* Canvas kennt nur 2 primitive Shapes
  * Rectangle
  * Path
* Path kann wie folgt erstellt werden
  * beginPath: Path erstellen
  * Path zusammen bauen
    * `arc`, `moveTo`, `lineTo`
  * Optional Path schliessen
    * `ctx.closePath()`
  * Render Variante auswählen
    * `stroke()`, `fill()`

### Width und Height

* Width und Height sind notwendig
* Die Width und Height geben die Grösse vom Inhalt an
  * Default: 320px auf 150px
* Bei unterschiedlicher Grösse (CSS und Canvas) wird die Grafik verzerrt

## Canvas: State

* Der Context hält sich ein State
  * State beinhaltet: Farbe, Font, Stroke, Transforms, ...
  * Der State bleibt bestehen bis überschrieben
* Context bietet Hilfsfunktionen an
  * `save()`
    * Speichert den kompletten State in einem Stack
  * `restore()`
    * Stellt den letzten State vom Stack wieder her
    * Sehr nützlich bei Transformationen
* [Drawing Shapes](https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial/Drawing_shapes)

### Beispiel

```js
ctx.save();
let max = 250;
for (let x = 50; x <= max; x += 50) {
  ctx.strokeStyle = "rgb(255, 0, 0)" // state ändern
  if (x === 150) {
    ctx.restore(); // state wiederherstellen
    ctx.lineWidth = 10 // state ändern
  }
  ctx.beginPath();
  ctx.arc(max + 5, max + 5, x, 0, Math.PI * 2);
  ctx.closePath();
  ctx.stroke();
}
```

### Transformations

Rotate and Translate != Translate and Rotate

```js
ctx.translate(50, 50);
ctx.save();

ctx.fillRect(-5, -5, 10, 10);
ctx.translate(50, 50); 
ctx.rotate(Math.PI / 4);
ctx.fillStyle = "rgb(255, 0, 0)";
ctx.fillRect(-5, -5, 10, 10);

ctx.restore();

ctx.rotate(Math.PI / 4); 
ctx.translate(50, 50);
ctx.fillStyle = "rgb(255, 0, 255)";
ctx.fillRect(-5, -5, 10, 10);
```



## Double Buffering

Ziel vom Double Buffering ist die Gewährleistung einer kontinuierlichen Bildfrequenz ohne Flackern.

1. Canvas 1 wird dargestellt
2. Canvas 2 wird neu gezeichnet
3. Canvas 1 und 2 werden ausgetauscht
   1. Alternative: Copy Canvas 2 nach Canvas 1

## Pre-Rendering

Objekte werden in einen nicht sichtbaren Canvas (offscreen canvas) gezeichnet. Dieser Canvas kann als Ganzes auf den sichtbaren Canvas (mehrfach) gerendert werden.

Pseudo-Code:

```js
let cloudCanvas = document.createElement("canvas"); // offscreen canvas
paintCloud(cloudCanvas); //zeichnet auf 0/0 die Wolke

// ohne pre-rendering
function drawField(){
  paintCloud(contextPlayfield, 100,50);
  paintCloud(contextPlayfield, 800,50);
}

// mit pre-rendering
function drawField(){
  contextPlayfield.drawImage(cloudCanvas, 100, 50); //Transformationen möglich
  contextPlayfield.drawImage(cloudCanvas, 800, 50); 
}
```



## Layering

* Statische Hintergründe möchte man nicht jedes Mal zeichnen
* Problem: Canvas 2d kennt keine Layers
* Lösung: 2 Canvas übereinander positionieren

```html
<canvas id="ground" width="600" height="600"></canvas>
<canvas id="playground" width="600" height="600"></canvas>

<style type="text/css">
  canvas { 
    border: 1px solid black; 
    position: absolute;
  }
</style>
```

## Canvas Animationen

Bei Canvas müssen Animationen manuell erstellt werden:

1. (Optional) Positionen von den Objekten bestimmen
2. Canvas löschen
3. Canvas neu zeichnen
4. Timer neu stellen

``` js
let counter = 1; 
function paint() {
  ctx.clearRect(0, 0, painting.width, painting.height);
  ctx.save();
  ctx.translate(100, 100);
  ctx.rotate(counter++ % 180 / 180 * Math.PI);
  ctx.fillRect(-50, -10, 100,20);
  ctx.restore();
  setTimeout(paint, 10);
}

setTimeout(paint, 10);
```

`window.requestAnimationFrame(callback)`

* Browser ruft vor jedem Repaint die Render-Funktion auf. Üblicherweise mit 60fps.
* Stoppt/Verlangsamt die Animation falls Window im Hintergrund ist
* [Details](https://developer.mozilla.org/en-US/docs/Web/API/window/requestAnimationFrame)
* Die Animationsschritte sollten nicht von der FPS abhängig sein
  * Beispiel: Bei jedem Tick wird das Bild um 1 Pixel verschoben
  * Schnelle CPU: 60fps => 60Pixel
  * Langsamere CPU: 30fps => 30Pixel
  * Lösung: Mit der Differenzzeit arbeiten

```js
let now = performance.now();
let length = 6000;
function paint(timestamp) {
  ctx.clearRect(0, 0, painting.width, painting.height);
  ctx.save();
  ctx.translate(100, 100);
  ctx.rotate(((timestamp - now) % length / length) * 2 * Math.PI);
  ctx.fillRect(-50, -10, 100, 20);
  ctx.restore();
  window.requestAnimationFrame(paint);
}
```

## Transitions and Animations

### Transitions

* Funktionieren nicht überall
* Die animierbaren [Properties](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_animated_properties) sind definiert
* Transition kann auf alle Properties angewendet werden: `transition: all .5s ease-in`
* Vorsicht ist mit «auto» Werten geboten
  * Sie lassen sich nicht immer animieren

```css
div {
  transition: <property> <duration> <timing-function> <delay>;
}

div {
  transition-property: font-size;
  transition-duration: 4s;
  transition-delay: 2s;
  font-size: 14px;
}
```

### Animations

* [Details MDN](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_Animations/Using_CSS_animations), [Details CSS-Tricks](https://css-tricks.com/almanac/properties/a/animation/)
* Komplexe Animation, welche aus mehreren Steps bestehen
* z.B. Einfliegen von neuen Elementen und am Ende die Opacity auf 1 ändern

``` css
.element { 
  animation: fadein 2s 1 linear;
}

@keyframes fadein {
  0% {
    position: relative; 
    left: -100px; 
    opacity: 0.2; 
  }
  
  99% { 
    opacity: 0.2; 
    display: inline-block;
  }
    
  100% { 
    opacity: 1; 
    position: relative; 
    left: 0px;
  }
}
```

### Unterschied

* Trigger
  * Transition: Wird ausgelöst, wenn sich CSS Properties ändern
  * Animation: Wird direkt ausgelöst z.B. beim Hinzufügen der CSS Klasse
* Looping
  * Bei Transition nicht möglich, bzw. nur mit JS oder anderen «Tricks»
  * Animation: Erlaubt es einen repeat count anzugeben
* JavaScript Support
  * Transition: Können die CSS Werte angepasst werden und die Transition läuft
  * Animation: [Mehr](https://css-tricks.com/controlling-css-animations-transitions-javascript/)
* [More Information](https://www.kirupa.com/html5/css3_animations_vs_transitions.htm)