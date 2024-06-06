#!/usr/bin/env python3

import os

list = os.listdir("assets/textures")
list.sort()

for f in list:
    name = f.split("-")[0].title()

    if name == "Map":
        split = f.split("-")
        name = split[1].title()

    print(f"Rocks.Add(RockKind.{name}, Raylib.LoadTexture(\"assets/textures/{f}\"));")
    # print(f"public Texture2D {name}Rock = Raylib.LoadTexture(\"assets/textures/{f}\");")

