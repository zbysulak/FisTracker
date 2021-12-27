<template>
  <div>
    <attendance-buttons class="desktop_none" />
    <v-row>
      <v-col class="pt-0 pb-0 col-xs-12 col-sm-3">
        <month-picker v-model="month" v-on:input="monthChange" />
      </v-col>
    </v-row>
    <v-row class="mt-0">
      <v-col class="col-sm-9 col-xs-12 mobile_none">
        <v-card-title>Table of attendance</v-card-title>
        <v-data-table
          :headers="headers"
          :items="timeInputs"
          class="elevation-1"
          :hide-default-footer="true"
          :item-class="workDayStyle"
          :items-per-page="-1">
          <template v-slot:[`item.date`]="{ item }">
            {{ item.date | formatDate }}
          </template>
          <template v-slot:[`item.homeOffice`]="{ item }">
            <v-icon v-if="item.homeOffice" class="text-center">
              mdi-checkbox-marked-circle-outline
            </v-icon>
          </template>
          <template class="item-center" v-slot:[`item.actions`]="{ item }">
            <v-icon small @click="editItem(item)"> mdi-pencil </v-icon>
          </template>
          <template v-slot:no-data>
            <p>No data</p>
          </template>
        </v-data-table>
        <div class="mt-5">
          <v-row class="pl-3">
            <time-edit-dialog v-model="editedItem" v-on:saved="updateTable">
            </time-edit-dialog>
          </v-row>
        </div>
      </v-col>
      <v-col class="col-sm-3 col-xs-12 mobile_block">
        <overview-panel
          :time="time"
          :loading="loading"
          class="mb-4"></overview-panel>
        <image-upload class="mobile_none" v-on:uploaded="reloadTable"></image-upload>
      </v-col>
    </v-row>
    <v-overlay :value="overlay">
      <v-progress-circular indeterminate size="64"></v-progress-circular>
    </v-overlay>
  </div>
</template>
<script>
import axios from "axios"
import OverviewPanel from "./OverviewPanel.vue"
import TimeEditDialog from "./TimeEditDialog.vue"
import MonthPicker from "./MonthPicker.vue"
import ImageUpload from "./ImageUpload.vue"
import AttendanceButtons from './AttendanceButtons.vue'

export default {
  components: { TimeEditDialog, OverviewPanel, MonthPicker, ImageUpload, AttendanceButtons },
  data: () => ({
    month: new Date().toISOString().substring(0, 7),
    dialog: false,
    dialogDelete: false,
    headers: [
      {
        text: "Date",
        value: "date"
      },
      { text: "Arrived at", value: "in.formatted" },
      { text: "Left at", value: "out.formatted" },
      { text: "Lunch break start", value: "lunchOut.formatted" },
      { text: "Lunch break end", value: "lunchIn.formatted" },
      { text: "Homeoffice / holiday", value: "homeOffice" },
      { text: "Actions", value: "actions", sortable: false }
    ],
    timeInputs: [],
    time: {},
    datePicker: false,
    editedIndex: -1,
    editedItem: {},
    defaultItem: {
      date: "",
      in: "08:00",
      lunchOut: null,
      lunchIn: null,
      out: null,
      homeOffice: false
    },
    overlay: false,
    loading: true
  }),

  watch: {
    dialog(val) {
      val || this.close()
    },
    dialogDelete(val) {
      val || this.closeDelete()
    }
  },

  created() {
    this.initialize()
  },

  methods: {
    reloadTable(e) {
      this.month = e
      this.updateTable()
    },
    monthChange(a) {
      this.loadTable(a)
    },
    updateTable() {
      this.loadTable(this.month)
    },
    loadTable(yearMonth) {
      const m = yearMonth.split("-")
      this.loading = true
      const year = m[0]
      const month = m[1]
      axios
        .get(
          this.appConfig.apiUrl +
            "/TimeInputs?month=" +
            month +
            "&year=" +
            year,
          {
            withCredentials: true
          }
        )
        .then((d) => {
          this.timeInputs = d.data.timeInputs
          this.time = d.data
        })
        .catch((d) => console.error(d))
        .finally(() => {
          this.loading = false
        })
    },
    workDayStyle(item) {
      return item.workDay ? "" : "grey lighten-3"
    },
    initialize() {
      this.loadTable(this.month)
    },
    deleteItem(item) {
      this.editedIndex = this.timeInputs.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialogDelete = true
    },
    deleteItemConfirm() {
      this.desserts.splice(this.editedIndex, 1)
      this.closeDelete()
    },
    editItem(item) {
      this.editedIndex = this.timeInputs.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialog = true
    },
    closeDelete() {
      this.dialogDelete = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    }
  }
}
</script>
