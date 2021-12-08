<template>
  <v-skeleton-loader
    v-if="loading"
    class="mx-auto"
    max-width="300"
    type="card"></v-skeleton-loader>
  <v-card v-else>
    <v-card-title>Month Overview</v-card-title>
    <v-card-text
      ><p class="mb-1">
        <span class="subheading mr-1">Month: </span>
        <b>{{ totalTime }}</b
        ><span> hours</span>
      </p>
      <p class="mb-1">
        <span class="subheading mr-1">Hours Left: </span>
        <b>{{ timeNeeded }}</b>
      </p>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  name: "RightPanel",
  props: { time: { type: Object, require: true } },
  components: {},

  computed: {
    loading() {
      return this.time == undefined || this.time.totalTime == undefined
    },
    totalTime() {
      if (this.time != undefined && this.time.totalTime != undefined) {
        return this.time?.totalTime?.totalHours
      } else {
        return "jeste nic"
      }
    },
    timeNeeded() {
      if (this.time != undefined && this.time.totalTime != undefined) {
        return (
          (this.time?.timeNeeded?.totalHours || 0) -
          this.time.totalTime.totalHours
        )
      } else {
        return "jeste nic"
      }
    }
  }
}
</script>
