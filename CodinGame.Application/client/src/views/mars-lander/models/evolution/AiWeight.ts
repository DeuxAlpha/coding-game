export class AiWeight {
  public HorizontalSpeedWeight!: number;
  public VerticalSpeedWeight!: number;
  public RotationWeight!: number;
  public HorizontalDistanceWeight!: number;
  public VerticalDistanceWeight!: number;
  public FuelWeight!: number;
  public BetterBias!: number; // Chance that an actor is picked from the 'good' pool.
  public BetterCutoff!: number; // What percentage of actors (ranked by score) are considered part of the 'good' pool.
  public MutationChance!: number; // Chance that an entirely new action will be generated.
  public ElitismBias!: number;

  constructor(init?: Partial<AiWeight>) {
    Object.assign(this, init);
  }
}