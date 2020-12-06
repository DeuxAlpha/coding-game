export class AiWeight {
  public HorizontalSpeedWeight!: number;
  public VerticalSpeedWeight!: number;
  public RotationWeight!: number;
  public VerticalDistanceWeight!: number;

  constructor(init?: Partial<AiWeight>) {
    Object.assign(this, init);
  }
}