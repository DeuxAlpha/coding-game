<template>
  <ALoadingOverlay :show="loading">
    <div class="flex flex-col items-center min-h-screen">
      <div class="flex flex-row items-baseline w-1/2">
        <label class="mx-1" for="selected-map">Map</label>
        <select class="mx-1 border border-gray-600" id="selected-map" v-model="selectedMap" @change="onMapChange">
          <option v-for="mapElement of mapElements" :value="mapElement.Name">{{ mapElement.Name }}</option>
        </select>
        <AButton :disabled="!selectedMap" @click="requestLanding">Land</AButton>
        <AButton :disabled="!selectedMap" @click="calculateWeights">Calculate Weights</AButton>
        <div class="ml-auto">
          <AButton @click="expandParams = !expandParams">
            <template v-if="expandParams">Hide Params</template>
            <template v-else>Show Params</template>
          </AButton>
        </div>
      </div>
      <div v-show="expandParams" class="flex flex-col items-baseline w-1/2 mt-2">
        <div class="flex flex-row w-full">
          <div class="w-1/3">
            <div class="flex flex-row">
              <label class="w-56" for="vertical-center-distance-weight">Horizontal Distance Weight</label>
              <input step="0.1" type="number" class="w-14 border border-gray-500" id="vertical-center-distance-weight"
                     v-model="horizontalDistanceWeight">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="vertical-distance-weight">Vertical Distance Weight</label>
              <input step="0.1" type="number" class="w-14 border border-gray-500" id="vertical-distance-weight"
                     v-model="verticalDistanceWeight">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="horizontal-speed-weight">Horizontal Speed Weight</label>
              <input step="0.1" type="number" class="w-14 border border-gray-500" id="horizontal-speed-weight"
                     v-model="horizontalSpeedWeight">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="vertical-speed-weight">Vertical Speed Weight</label>
              <input step="0.1" type="number" class="w-14 border border-gray-500" id="vertical-speed-weight"
                     v-model="verticalSpeedWeight">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="rotation-weight">Rotation Weight</label>
              <input step="0.1" type="number" class="w-14 border border-gray-500" id="rotation-weight"
                     v-model="rotationWeight">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="fuel-weight">Fuel Weight</label>
              <input step="0.1" type="number" class="w-14 border border-gray-500" id="fuel-weight"
                     v-model="fuelWeight">
            </div>
          </div>
          <div class="w-1/3">
            <div class="flex flex-row">
              <label class="w-56" for="generation-request">Generations</label>
              <input type="number" class="w-14 border border-gray-500" id="generation-request"
                     v-model="generationRequest">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="population-request">Population</label>
              <input type="number" class="w-14 border border-gray-500" id="population-request"
                     v-model="populationRequest">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="max-actions">Max Actions</label>
              <input type="number" class="w-14 border border-gray-500" id="max-actions" v-model="maxActions">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="better-bias">Better Bias</label>
              <input step="0.01" type="number" class="w-14 border border-gray-500" id="better-bias"
                     v-model="betterBias">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="better-cutoff">Better Cutoff</label>
              <input step="0.01" type="number" class="w-14 border border-gray-500" id="better-cutoff"
                     v-model="betterCutoff">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="mutation-chance">Mutation Chance</label>
              <input step="0.01" type="number" class="w-14 border border-gray-500" id="mutation-chance"
                     v-model="mutationChance">
            </div>
            <div class="flex flex-row">
              <label class="w-56" for="elitism-bias">Elitism Bias</label>
              <input step="0.01" type="number" class="w-14 border border-gray-500" id="elitism-bias"
                     v-model="elitismBias">
            </div>
          </div>
          <div class="w-1/3">
            <div class="flex flex-row">
              <label for="initial-fuel" class="w-56">Initial Fuel</label>
              <input type="number" class="w-14 border border-gray-500" id="initial-fuel" v-model="initialFuel">
            </div>
            <div class="flex flex-row">
              <label for="initial-power" class="w-56">Initial Power</label>
              <input type="number" class="w-14 border border-gray-500" id="initial-power" v-model="initialPower">
            </div>
            <div class="flex flex-row">
              <label for="initial-rotation" class="w-56">Initial Rotation</label>
              <input type="number" class="w-14 border border-gray-500" id="initial-rotation" v-model="initialRotation">
            </div>
            <div class="flex flex-row">
              <label for="initial-x" class="w-56">Initial X</label>
              <input type="number" class="w-14 border border-gray-500" id="initial-x" v-model="initialX">
            </div>
            <div class="flex flex-row">
              <label for="initial-y" class="w-56">Initial Y</label>
              <input type="number" class="w-14 border border-gray-500" id="initial-y" v-model="initialY">
            </div>
            <div class="flex flex-row">
              <label for="initial-h-speed" class="w-56">Initial Horizontal Speed</label>
              <input type="number" class="w-14 border border-gray-500" id="initial-h-speed"
                     v-model="initialHorizontalSpeed">
            </div>
            <div class="flex flex-row">
              <label for="initial-v-speed" class="w-56">Initial Vertical Speed</label>
              <input type="number" class="w-14 border border-gray-500" id="initial-v-speed"
                     v-model="initialVerticalSpeed">
            </div>
          </div>
        </div>
      </div>
      <div id="map" ref="map"/>
      <div class="flex flex-row justify-between" v-show="selectedGeneration > 0">
        <label class="mx-1" for="generation">Generation</label>
        <select class="mx-1 border border-gray-600" id="generation" v-model="selectedGeneration">
          <option v-for="generation in generations" :key="generation.GenerationNumber"
                  :value="generation.GenerationNumber">
            {{ generation.GenerationNumber }}
          </option>
        </select>
        <button @click="onSubmitGenerationClicked">Submit</button>
      </div>
      <div class="flex flex-col w-8/12" v-show="selectedGeneration > 0">
        <AButton class="ml-auto w-48" @click="applyBestScoringActor">Apply best Actor</AButton>
        <div class="flex flex-row w-full">
          <div class="w-full">Score</div>
          <div class="w-full">X</div>
          <div class="w-full">Y</div>
          <div class="w-full">HorizontalSpeed</div>
          <div class="w-full">VerticalSpeed</div>
          <div class="w-full">Rotation</div>
          <div class="w-full">Power</div>
          <div class="w-full">Fuel</div>
          <div class="w-full"></div>
        </div>
        <div class="flex flex-row w-full" v-for="(actor, index) of orderedActors" :key="index">
          <div class="w-full">{{ actor.Score }}</div>
          <div class="w-full">{{ actor.Lander.Situation.X }}</div>
          <div class="w-full">{{ actor.Lander.Situation.Y }}</div>
          <div class="w-full">{{ actor.Lander.Situation.HorizontalSpeed }}</div>
          <div class="w-full">{{ actor.Lander.Situation.VerticalSpeed }}</div>
          <div class="w-full">{{ actor.Lander.Situation.Rotation }}</div>
          <div class="w-full">{{ actor.Lander.Situation.Power }}</div>
          <div class="w-full">{{ actor.Lander.Situation.Fuel }}</div>
          <div class="w-full">
            <button class="bg-blue-200" @click="copyActions(index)">Copy Actions</button>
          </div>
        </div>
      </div>
    </div>
  </ALoadingOverlay>
</template>

<script lang="ts">
import {defineComponent, Ref, ref} from 'vue';
import {MarsLanderApi} from "@/views/mars-lander/api/MarsLanderApi";
import {Map} from "@/views/mars-lander/models/environment/Map";
import * as Echarts from "echarts";
import AButton from "@/components/simple/AButton.vue";
import {Generation} from "@/views/mars-lander/models/evolution/Generation";
import {LandRequest} from "@/views/mars-lander/api/requests/LandRequest";
import {AiWeight} from "@/views/mars-lander/models/evolution/AiWeight";
import {EvolutionParameters} from "@/views/mars-lander/models/evolution/EvolutionParameters";
import {LanderStatus} from "@/views/mars-lander/models/evolution/LanderStatus";
import ALoadingOverlay from "@/components/utilities/ALoadingOverlay.vue";
import {GenerationActor} from "@/views/mars-lander/models/evolution/GenerationActor";
import {CalculateWeightRequest} from "@/views/mars-lander/api/requests/CalculateWeightRequest";

export default defineComponent({
  name: 'CMarsLander',
  components: {ALoadingOverlay, AButton},
  async setup() {
    const map = ref(null) as Ref<null | HTMLDivElement>;
    const selectedMap = ref('');
    const horizontalSpeedWeight = ref(1);
    const verticalSpeedWeight = ref(1);
    const rotationWeight = ref(1);
    const horizontalDistanceWeight = ref(1);
    const verticalDistanceWeight = ref(1);
    const fuelWeight = ref(1);
    const generationRequest = ref(100);
    const populationRequest = ref(100);
    const maxActions = ref(-1);
    const initialFuel = ref(0);
    const initialPower = ref(0);
    const initialRotation = ref(0);
    const initialX = ref(0);
    const initialY = ref(0);
    const initialHorizontalSpeed = ref(0);
    const initialVerticalSpeed = ref(0);
    const betterBias = ref(0.8);
    const betterCutoff = ref(0.2);
    const mutationChance = ref(0.05);
    const elitismBias = ref(0.2);
    const expandParams = ref(false);
    const loading = ref(false);

    const orderedActors = ref([]) as Ref<GenerationActor[]>;

    const marsLanderApi = new MarsLanderApi();

    const graph = ref(null) as Ref;

    const mapElements = await marsLanderApi.GetMaps();

    const selectedGeneration = ref(0);

    function getMapToRender() {
      return mapElements.find(element => element.Name === selectedMap.value) as Map;
    }

    function onMapChange() {
      selectedGeneration.value = 0;
      const mapToRender = getMapToRender();
      initialFuel.value = mapToRender.InitialFuel;
      initialPower.value = mapToRender.InitialPower;
      initialRotation.value = mapToRender.InitialRotation
      initialX.value = mapToRender.InitialX;
      initialY.value = mapToRender.InitialY;
      initialHorizontalSpeed.value = mapToRender.InitialHorizontalSpeed;
      initialVerticalSpeed.value = mapToRender.InitialVerticalSpeed;

      renderMap(mapToRender);
    }

    const generations = ref([]) as Ref<Array<Generation>>;

    function renderMap(mapToRender: Map) {
      if (graph.value === null) {
        graph.value = Echarts.init(map.value as HTMLDivElement);
      }
      graph.value?.clear();
      const surfaceArray = mapToRender.SurfaceZones.map(element => [element.LeftX, element.LeftY]);
      surfaceArray.push([mapToRender.SurfaceZones.last().RightX, mapToRender.SurfaceZones.last().RightY]);
      const generationActors = generations.value?.find(g => g.GenerationNumber === selectedGeneration.value) as Generation | null;
      const chartSeries: any[] = [];
      chartSeries.push({ // Surface
        type: 'line',
        symbol: 'none',
        lineStyle: {
          color: '#8d0d0d',
          width: 2
        },
        data: surfaceArray,
      });
      for (const actor of generationActors?.Actors ?? []) {
        chartSeries.push({ // Actors in generation
          type: 'line',
          lineStyle: {
            width: 1
          },
          symbol: 'arrow',
          symbolSize: 6,
          symbolRotate: (value, params) => {
            return params.data?.rotation ?? 0;
          },
          data: actor.Lander.Situations.map((situation, index) => {
            return {
              rotation: situation.Rotation,
              value: [situation.X, situation.Y],
              score: actor.Score,
              action: actor.Lander.Actions[index],
              fuel: situation.Fuel,
              power: situation.Power,
              horizontalSpeed: situation.HorizontalSpeed,
              verticalSpeed: situation.VerticalSpeed,
              status: LanderStatus[actor.Lander.Status]
            }
          }),
          tooltip: {
            formatter: (params) => {
              return `X: ${params.data.value[0]} Y: ${params.data.value[1]}<br/>
              Rotation: ${params.data.rotation} Power: ${params.data.power} Fuel: ${params.data.fuel}<br/>
              HSpeed: ${params.data.horizontalSpeed} VSpeed: ${params.data.verticalSpeed}<br/>
              Action: ${params.data.action} <b>Score: ${params.data.score}</b><br/>
              Status: ${params.data.status} Actions: ${actor.Lander.Actions.length}`;
            }
          }
        })
      }
      const chartOptions = {
        title: {
          text: mapToRender.Name
        },
        xAxis: {
          min: 0,
          max: 7000
        },
        yAxis: {
          min: 0,
          max: 3000
        },
        tooltip: {
          trigger: 'item',
          hideDelay: 500,
          enterable: true
        },
        series: chartSeries,
      } as Echarts.EChartsOption;
      graph.value.setOption(chartOptions);

    }

    async function requestLanding() {
      const mapToRender = getMapToRender();
      mapToRender.InitialVerticalSpeed = parseInt(initialVerticalSpeed.value.toString());
      mapToRender.InitialHorizontalSpeed = parseInt(initialHorizontalSpeed.value.toString());
      mapToRender.InitialY = parseInt(initialY.value.toString());
      mapToRender.InitialX = parseInt(initialX.value.toString());
      mapToRender.InitialRotation = parseInt(initialRotation.value.toString());
      mapToRender.InitialPower = parseInt(initialPower.value.toString());
      mapToRender.InitialFuel = parseInt(initialFuel.value.toString());

      loading.value = true;
      generations.value = await marsLanderApi.Land(
          new LandRequest({
            Map: mapToRender,
            AiWeight: new AiWeight({
              HorizontalSpeedWeight: parseFloat(horizontalSpeedWeight.value.toString()),
              VerticalSpeedWeight: parseFloat(verticalSpeedWeight.value.toString()),
              RotationWeight: parseFloat(rotationWeight.value.toString()),
              HorizontalDistanceWeight: parseFloat(horizontalDistanceWeight.value.toString()),
              ElitismBias: parseFloat(elitismBias.value.toString()),
              BetterBias: parseFloat(betterBias.value.toString()),
              BetterCutoff: parseFloat(betterCutoff.value.toString()),
              MutationChance: parseFloat(mutationChance.value.toString())
            }),
            Parameters: new EvolutionParameters({
              Generations: parseFloat(generationRequest.value.toString()),
              Population: parseFloat(populationRequest.value.toString()),
              Actions: parseFloat(maxActions.value.toString())
            })
          })).finally(() => {
        loading.value = false;
      });
      selectedGeneration.value = generations.value.length;

      setOrderedLanders()
      renderMap(mapToRender);
    }

    function setOrderedLanders() {
      orderedActors.value = generations.value[selectedGeneration.value - 1].Actors.sort((a, b) => {
        return a.Score - b.Score;
      }).map(actor => {
        return new GenerationActor({
          Score: actor.Score,
          Lander: actor.Lander
        });
      });
    }

    function onSubmitGenerationClicked() {
      setOrderedLanders()
      renderMap(getMapToRender());
    }

    function applyBestScoringActor() {
      initialFuel.value = orderedActors.value[0].Lander.Situation.Fuel;
      initialPower.value = orderedActors.value[0].Lander.Situation.Power;
      initialRotation.value = orderedActors.value[0].Lander.Situation.Rotation;
      initialX.value = orderedActors.value[0].Lander.Situation.X;
      initialY.value = orderedActors.value[0].Lander.Situation.Y;
      initialHorizontalSpeed.value = orderedActors.value[0].Lander.Situation.HorizontalSpeed;
      initialVerticalSpeed.value = orderedActors.value[0].Lander.Situation.VerticalSpeed;
    }

    function copyActions(index: number) {
      let stacks: string[] = [];
      for (const situation of orderedActors.value[index].Lander.Situations) {
        stacks.push(`new[] {${situation.Rotation}, ${situation.Power}}\r\n`)
      }
      stacks.push(`new[] {${orderedActors.value[index].Lander.Situation.Rotation}, ${orderedActors.value[index].Lander.Situation.Power}}\r\n`)
      navigator.clipboard.writeText(stacks.toString());
    }

    async function calculateWeights() {
      const map = getMapToRender();
      loading.value = true;
      await marsLanderApi.CalculateWeights(new CalculateWeightRequest({
        Map: map,
        OriginalWeights: new AiWeight({
          HorizontalSpeedWeight: parseFloat(horizontalSpeedWeight.value.toString()),
          VerticalSpeedWeight: parseFloat(verticalSpeedWeight.value.toString()),
          RotationWeight: parseFloat(rotationWeight.value.toString()),
          HorizontalDistanceWeight: parseFloat(horizontalDistanceWeight.value.toString()),
          VerticalDistanceWeight: parseFloat(verticalDistanceWeight.value.toString()),
          FuelWeight: parseFloat(fuelWeight.value.toString()),
          ElitismBias: parseFloat(elitismBias.value.toString()),
          BetterBias: parseFloat(betterBias.value.toString()),
          BetterCutoff: parseFloat(betterCutoff.value.toString()),
          MutationChance: parseFloat(mutationChance.value.toString())
        }),
        Parameters: new EvolutionParameters({
          Generations: parseFloat(generationRequest.value.toString()),
          Population: parseFloat(populationRequest.value.toString()),
          Actions: parseFloat(maxActions.value.toString())
        }),
        ChangePerTry: 0.1,
        DownwardTries: 10,
        UpwardTries: 200
      })).finally(() => {
        loading.value = false;
      })
    }

    return {
      calculateWeights,
      copyActions,
      mapElements,
      selectedMap,
      onMapChange,
      map,
      requestLanding,
      selectedGeneration,
      horizontalSpeedWeight,
      horizontalDistanceWeight,
      verticalDistanceWeight,
      fuelWeight,
      elitismBias,
      verticalSpeedWeight,
      rotationWeight,
      expandParams,
      generations,
      onSubmitGenerationClicked,
      generationRequest,
      populationRequest,
      maxActions,
      initialFuel,
      initialPower,
      initialRotation,
      initialX,
      initialY,
      initialHorizontalSpeed,
      initialVerticalSpeed,
      loading,
      orderedActors,
      applyBestScoringActor,
      betterBias,
      betterCutoff,
      mutationChance
    };
  }
})
</script>

<style scoped lang="scss">
#map {
  width: 100vw;
  height: 90vh;
}

.v-enter-active, .v-leave-active {
  transition: height .5s;
  overflow: hidden;
}

.v-enter .v-leave-to {
  height: 0;
}
</style>