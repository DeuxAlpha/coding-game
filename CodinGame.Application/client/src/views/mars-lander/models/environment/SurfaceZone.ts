import {SurfaceElement} from "@/views/mars-lander/models/environment/SurfaceElement";

export class SurfaceZone {
  public LeftX!: number;
  public LeftY!: number;
  public RightX!: number;
  public RightY!: number;
  public Angle!: number;
  public SurfaceElements!: SurfaceElement[];
}