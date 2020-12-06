<template>
  <div class="flex flex-col items-center min-h-screen">
    <div class="flex flex-row items-baseline w-1/2">
      <label class="mx-1" for="selected-map">Map</label>
      <select class="mx-1 border border-gray-600" id="selected-map" v-model="selectedMap" @change="onMapChange">
        <option v-for="mapElement of mapElements" :value="mapElement.Name">{{ mapElement.Name }}</option>
      </select>
      <AButton :disabled="!selectedMap" @click="requestLanding">Land</AButton>

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
            <label class="w-56" for="horizontal-speed-weight">Horizontal Speed Weight</label>
            <input type="number" class="w-14 border border-gray-500" id="horizontal-speed-weight" v-model="horizontalSpeedWeight">
          </div>
          <div class="flex flex-row">
            <label class="w-56" for="vertical-speed-weight">Vertical Speed Weight</label>
            <input type="number" class="w-14 border border-gray-500" id="vertical-speed-weight" v-model="verticalSpeedWeight">
          </div>
          <div class="flex flex-row">
            <label class="w-56" for="rotation-weight">Rotation Weight</label>
            <input type="number" class="w-14 border border-gray-500" id="rotation-weight" v-model="rotationWeight">
          </div>
          <div class="flex flex-row">
            <label class="w-56" for="vertical-distance-weight">Vertical Distance Weight</label>
            <input type="number" class="w-14 border border-gray-500" id="vertical-distance-weight" v-model="verticalDistanceWeight">
          </div>
        </div>
        <div class="w-1/3">
          <div class="flex flex-row">
            <label class="w-56" for="generation-request">Generations</label>
            <input type="number" class="w-14 border border-gray-500" id="generation-request" v-model="generationRequest">
          </div>
          <div class="flex flex-row">
            <label class="w-56" for="population-request">Population</label>
            <input type="number" class="w-14 border border-gray-500" id="population-request" v-model="populationRequest">
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
  </div>
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

export default defineComponent({
  name: 'CMarsLander',
  components: {AButton},
  async setup() {
    const map = ref(null) as Ref<null | HTMLDivElement>;
    const selectedMap = ref('');
    const horizontalSpeedWeight = ref(1);
    const verticalSpeedWeight = ref(1);
    const rotationWeight = ref(1);
    const verticalDistanceWeight = ref(1);
    const generationRequest = ref(100);
    const populationRequest = ref(100);
    const expandParams = ref(false);

    const marsLanderApi = new MarsLanderApi();

    const graph = ref(null) as Ref;

    const mapElements = await marsLanderApi.GetMaps();

    const selectedGeneration = ref(0);

    function onMapChange() {
      selectedGeneration.value = 0;

      renderMap(true);
    }

    const generations = ref([]) as Ref<Array<Generation>>;

    function renderMap(reset = false) {
      if (graph.value === null) {
        graph.value = Echarts.init(map.value as HTMLDivElement);
      }
      if (reset)
        graph.value?.clear();
      const mapToRender = mapElements.find(element => element.Name === selectedMap.value) as Map;
      const surfaceArray = mapToRender.SurfaceElements.map(element => [element.X, element.Y]);
      const generationActors = generations.value?.find(g => g.GenerationNumber === selectedGeneration.value) as Generation | null;
      const chartSeries: any[] = [];
      chartSeries.push({ // Surface
        type: 'line',
        symbol: 'none',
        lineStyle: {
          color: '#8d0d0d',
          width: 2
        },
        data: surfaceArray
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
              Action: ${params.data.action} Score: ${params.data.score}<br/>
              Status: ${params.data.status}`;
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
          trigger: 'item'
        },
        series: chartSeries
      } as Echarts.EChartsOption;
      console.dir(chartOptions);
      graph.value.setOption(chartOptions);

    }

    async function requestLanding() {
      generations.value = await marsLanderApi.Land(
          new LandRequest({
            Map: mapElements.find(element => element.Name === selectedMap.value),
            AiWeight: new AiWeight({
              HorizontalSpeedWeight: parseFloat(horizontalSpeedWeight.value.toString()),
              RotationWeight: parseFloat(rotationWeight.value.toString()),
              VerticalDistanceWeight: parseFloat(verticalDistanceWeight.value.toString()),
              VerticalSpeedWeight: parseFloat(verticalSpeedWeight.value.toString()),
            }),
            Parameters: new EvolutionParameters({
              Generations: parseFloat(generationRequest.value.toString()),
              Population: parseFloat(populationRequest.value.toString()),
            })
          }));
      selectedGeneration.value = 1;

      renderMap();
    }

    function onSubmitGenerationClicked() {
      renderMap();
    }

    return {
      mapElements,
      selectedMap,
      onMapChange,
      map,
      requestLanding,
      selectedGeneration,
      horizontalSpeedWeight,
      verticalSpeedWeight,
      rotationWeight,
      verticalDistanceWeight,
      expandParams,
      generations,
      onSubmitGenerationClicked,
      generationRequest,
      populationRequest
    };
  }
})
</script>

<style scoped lang="scss">
#map {
  width: 1200px;
  height: 720px;
}

.v-enter-active, .v-leave-active {
  transition: height .5s;
  overflow: hidden;
}

.v-enter .v-leave-to {
  height: 0;
}
</style>