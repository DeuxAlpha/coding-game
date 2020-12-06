<template>
  <div class="overlay">
    <div class="z-10 absolute w-full h-full bg-black bg-opacity-70 flex justify-center items-center" v-if="show">
      <slot name="overlay-top">
        <h1 class="font-bold text-blue-400 text-4xl">Loading...</h1>
      </slot>
    </div>
    <div class="content" :class="OverlayVisible">
      <slot/>
    </div>
  </div>
</template>

<script lang="ts">
import {defineComponent, computed} from 'vue';

export default defineComponent({
  name: 'ALoadingOverlay',
  props: {
    show: {
      type: Boolean,
      default: false
    }
  },
  setup(props) {
    const overlayVisible = computed(() => {
      return props.show ? 'show' : '';
    })

    return {
      overlayVisible
    }
  }
})
  // get OverlayVisible(): string {
  //   return this.overlay ? 'show' : '';
  // }
</script>

<style scoped lang="scss">
.overlay {
  position: relative;

  .show {
    &:after {
      content: '\A';
      position: absolute;
      width: 100%;
      height: 100%;
      top: 0;
      left: 0;
      background: rgba(0, 0, 0, 0.4);
    }
  }
}

.absolute-horizontal-center {
  left: 50%;
  transform: translateX(-50%);
}

.absolute-vertical-center {
  top: 50%;
  transform: translateY(-50%);
}

</style>