* put all icons which will be used in SVG sprite
* icons should have one layer
* do not put icons from the top part of a site if they do not use anywhere else

# How to use sprite

1. put html code into view
2. set minimum width and height attributes
3. set right xlink:href name
4. check focusable="false" (fix IE bug)

example: 
```html
<svg class="icon" width="18" height="18" focusable="false">
    <use xlink:href="#icon-cross"></use>
</svg>
```

