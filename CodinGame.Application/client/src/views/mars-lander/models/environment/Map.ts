import {SurfaceZone} from "@/views/mars-lander/models/environment/SurfaceZone";

export class Map {
  public Name!: string;
  public SurfaceZones!: SurfaceZone[];
  public InitialX!: number;
  public InitialY!: number;
  public InitialHorizontalSpeed!: number;
  public InitialVerticalSpeed!: number;
  public InitialFuel!: number;
  public InitialRotation!: number;
  public InitialPower!: number;
}