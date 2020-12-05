import {MapElement} from "@/views/mars-lander/models/MapElement";

export class Map {
  public Name!: string;
  public SurfaceElements!: MapElement[];
  public InitialX!: number;
  public InitialY!: number;
  public InitialHorizontalSpeed!: number;
  public InitialVerticalSpeed!: number;
  public InitialFuel!: number;
  public InitialRotation!: number;
  public InitialPower!: number;
}